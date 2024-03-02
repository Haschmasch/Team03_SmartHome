using MainUnit.HttpClients;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Room;
using MainUnit.Models.RoomTemperature;
using MainUnit.Models.Settings;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MainUnit.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMongoCollection<Room> _roomCollection;
        private readonly IMongoCollection<ThermostatWithURL> _thermostatCollection;
        private readonly IMongoCollection<RoomTemperatureEntry> _roomTemperatureEntries;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IOptions<MongoDbSettings> settings, ILogger<RoomService> logger)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _roomCollection = mongoDatabase.GetCollection<Room>(
                settings.Value.RoomCollectionName);

            _thermostatCollection = mongoDatabase.GetCollection<ThermostatWithURL>(
                settings.Value.ThermostatCollectionName);

            _roomTemperatureEntries = mongoDatabase.GetCollection<RoomTemperatureEntry>(
                settings.Value.RoomTemperatureCollectionName);
            this._logger = logger;
        }

        public Room AddRoom(BaseRoom room)
        {
            Room roomCopy = new Room(room);
            _roomCollection.InsertOne(roomCopy);
            var temp = _roomCollection.Find(r => r.Id == roomCopy.Id).FirstOrDefault();
            return temp;
        }

        public IList<Room> GetRooms(int skip, int limit)
        {
            var result = _roomCollection.AsQueryable();
            result = result.Skip(skip).Take(limit);
            return result.ToList();
        }

        public Room GetRoom(string id)
        {
            var room = CheckRoomExists(id);
            return room;
        }

        public Room UpdateRoom(BaseRoom room)
        {
            var roomCopy = CheckRoomExists(room.Id!);
            roomCopy.Name = room.Name;
            roomCopy.Description = room.Description;
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, room.Id);
            var result = _roomCollection.ReplaceOne(filter, roomCopy);
            if (result.IsAcknowledged)
                return roomCopy;
            else
                throw new RoomNotFoundException($"Room with Id:'{room.Id}' not found.");
        }

        public void RemoveRoom(string id)
        {
            var room = CheckRoomExists(id);

            //Remove roomIds of thermostats associated with room
            foreach (var thermostatId in room.ThermostatIds)
            {
                FilterDefinition<ThermostatWithURL> thermostatFilter = Builders<ThermostatWithURL>.Filter.Eq(r => r.Id, thermostatId);
                UpdateDefinition<ThermostatWithURL> thermostatUpdate = Builders<ThermostatWithURL>.Update.Set(t => t.RoomId, null);
                var thermostatUpdateResult = _thermostatCollection.UpdateOne(thermostatFilter, thermostatUpdate);
            }

            _roomCollection.DeleteOne(x => x.Id == id);
        }

        public Room AddThermostat(string roomId, string thermostatId)
        {
            var room = CheckRoomExists(roomId);
            if (room.ThermostatIds.Contains(thermostatId))
            {
                throw new ThermostatExistsException($"Thermostat with id:'{thermostatId}' is already added to room.");
            }

            var thermostat = CheckThermostatExists(thermostatId);
            if (thermostat.RoomId != null && thermostat.RoomId != String.Empty && thermostat.RoomId != roomId)
            {
                //Check if room assigned to thermostat really exists. If it does, throw.
                if (_roomCollection.Find(r => r.Id == thermostat.RoomId).FirstOrDefault() != null)
                    throw new RoomExistsException($"Thermostat already assigned to room with Id:'{thermostat.RoomId}'");
            }

            FilterDefinition<ThermostatWithURL> thermostatFilter = Builders<ThermostatWithURL>.Filter.Eq(r => r.Id, thermostatId);
            UpdateDefinition<ThermostatWithURL> thermostatUpdate = Builders<ThermostatWithURL>.Update.Set(t => t.RoomId, roomId);
            var thermostatUpdateResult = _thermostatCollection.UpdateOne(thermostatFilter, thermostatUpdate);

            room.ThermostatIds.Add(thermostatId);

            FilterDefinition<Room> roomFilter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            UpdateDefinition<Room> roomUpdate = Builders<Room>.Update.AddToSet(r => r.ThermostatIds, thermostatId);
            var updateResult = _roomCollection.UpdateOne(roomFilter, roomUpdate);

            if (updateResult.IsAcknowledged)
                return room;
            else
                throw new RoomNotFoundException($"Room with Id:'{roomId}' not found.");
        }

        public void RemoveThermostat(string roomId, string thermostatId)
        {
            var room = CheckRoomExists(roomId);

            var thermostat = CheckThermostatExists(thermostatId);
            //Remove association from thermostat
            thermostat.RoomId = null!;
            room.ThermostatIds.Remove(thermostatId);

            FilterDefinition<ThermostatWithURL> thermFilter = Builders<ThermostatWithURL>.Filter.Eq(r => r.Id, thermostatId);
            _thermostatCollection.FindOneAndReplace(thermFilter, thermostat);

            FilterDefinition<Room> roomFilter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            _roomCollection.FindOneAndReplace(roomFilter, room);
        }

        public Room SetRoomTemperature(string roomId, float temperature)
        {
            var room = CheckRoomExists(roomId);

            //Update individual thermostat temperatures by calling their API
            foreach (var id in room.ThermostatIds)
            {
                var thermostat = _thermostatCollection.Find(t => t.Id == id).FirstOrDefault();
                ThermostatClient client = null!;
                if (thermostat == null)
                {
                    //No exception is thrown here, because this is mainly a paranoia check.
                    _logger.LogWarning($"Room:'{room.Id}' has a Thermostat assigned, which does not exist in {nameof(_thermostatCollection)}. ThermostatId:'{id}'");
                    continue;
                }
                client = new ThermostatClient(thermostat.URL);
                thermostat.Temperature = temperature;
                var task = client.UpdateThermostatAsync(thermostat);
                task.GetAwaiter().GetResult();
            }

            //Update and return room
            room.Temperature = temperature;
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            UpdateDefinition<Room> update = Builders<Room>.Update.Set("Temperature", temperature);
            _roomCollection.UpdateOne(filter, update);

            //Add temperature change to timeseries
            _roomTemperatureEntries.InsertOne(new RoomTemperatureEntry()
            {
                Metadata = new RoomTempMetadataEntry()
                {
                    RoomId = roomId,
                    ThermostatIds = room.ThermostatIds
                },
                Temperature = temperature
            });

            return room;
        }

        private Room CheckRoomExists(string roomId)
        {
            Room result = null!;
            try
            {
                result = _roomCollection.Find(t => t.Id == roomId).FirstOrDefault();
            }
            catch (FormatException ex)
            {
                throw new InvalidIdException($"No room found for id:'{roomId}'", ex);
            }

            if (result == null)
            {
                throw new RoomNotFoundException($"Room with Id:'{roomId}' not found.");
            }

            return result;
        }

        private ThermostatWithURL CheckThermostatExists(string thermostatId)
        {
            ThermostatWithURL result = null!;
            try
            {
                result = _thermostatCollection.Find(t => t.Id == thermostatId).FirstOrDefault();
            }
            catch (FormatException ex)
            {
                throw new InvalidIdException($"No thermostat found for id:'{thermostatId}'", ex);
            }

            if (result == null)
            {
                throw new ThermostatNotFoundException($"Thermostat with Id:'{thermostatId}' not found.");
            }

            return result;
        }
    }
}
