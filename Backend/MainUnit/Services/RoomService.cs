using MainUnit.Controllers;
using MainUnit.HttpClients;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Room;
using MainUnit.Models.RoomTemperature;
using MainUnit.Models.Settings;
using MainUnit.Models.Thermostat;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Runtime.InteropServices;

namespace MainUnit.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMongoCollection<Room> _roomCollection;
        private readonly IMongoCollection<ThermostatWithURL> _thermostatCollection;
        private readonly IMongoCollection<RoomTemperatureEntry> _roomTemperatureEntries;

        public RoomService(IOptions<MongoDbSettings> settings)
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
        }

        public Room AddRoom(Room room)
        {
            _roomCollection.InsertOne(room);
            return _roomCollection.Find(r => r.Id == room.Id).FirstOrDefault();
        }

        public List<Room> GetRooms(int skip, int limit)
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

        public Room UpdateRoom(Room room)
        {
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, room.Id);
            var result = _roomCollection.ReplaceOne(filter, room);
            if (result.IsAcknowledged)
                return room;
            else
                throw new RoomNotFoundException($"Room {room.Id} not found.");
        }

        public void RemoveRoom(string id)
        {
            CheckRoomExists(id);

            _roomCollection.DeleteOne(x => x.Id == id);
        }

        public Room AddThermostat(string roomId, string thermostatId)
        {
            var room = CheckRoomExists(roomId);

            CheckThermostatExists(thermostatId);

            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet(r => r.ThermostatIds, thermostatId);
            var updateResult = _roomCollection.UpdateOne(filter, update);
            room.ThermostatIds.Add(thermostatId);
            if (updateResult.IsAcknowledged)
                return room;
            else
                throw new RoomNotFoundException($"Room {room.Id} not found.");
        }

        public void RemoveThermostat(string roomId, string thermostatId)
        {
            var room = CheckRoomExists(roomId);

            CheckThermostatExists(thermostatId);

            room.ThermostatIds.Remove(thermostatId);

            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            var result = _roomCollection.FindOneAndReplace(filter, room);

            if (result == null)
                throw new RoomNotFoundException($"Room {room.Id} not found.");
        }

        public Room SetRoomTemperature(string roomId, float temperature)
        {
            var room = CheckRoomExists(roomId);

            //Update individual thermostat temperatures by calling their API
            foreach (var id in room.ThermostatIds)
            {
                var thermostat = _thermostatCollection.Find(t => t.Id == id).FirstOrDefault();
                ThermostatClient client = null;
                if (thermostat == null)
                {
                    //No exception is thrown here, because this is mainly a paranoia check.
                    //TODO add logging
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
                    Id = roomId,
                    ThermostatIds = room.ThermostatIds
                },
                Temperature = temperature
            });

            return room;
        }

        private Room CheckRoomExists(string roomId)
        {
            var result = _roomCollection.Find(t => t.Id == roomId);

            if (result == null || !result.Any())
            {
                throw new RoomNotFoundException($"Room {roomId} not found.");
            }

            return result.FirstOrDefault();
        }

        private Thermostat CheckThermostatExists(string thermostatId)
        {
            var result = _thermostatCollection.Find(t => t.Id == thermostatId);

            if (result == null || !result.Any())
            {
                throw new ThermostatNotFoundException($"Room {thermostatId} not found.");
            }

            return result.FirstOrDefault();
        }
    }
}
