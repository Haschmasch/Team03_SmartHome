using MainUnit.HttpClients;
using MainUnit.Models;
using MainUnit.Models.Exceptions;
using MainUnit.Models.Settings;
using MainUnit.Services.Interfaces;
using Microsoft.Extensions.Options;
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
        }

        public void AddRoom(Room room)
        {
            _roomCollection.InsertOne(room);
        }

        public List<Room> GetRooms(int skip, int limit)
        {
            var result = _roomCollection.AsQueryable();
            result = result.Skip(skip).Take(limit);
            return result.ToList();
        }

        public Room GetRoom(int id)
        {
            var result = _roomCollection.Find(t => t.Id == id);

            if (result != null && result.Any())
            {
                return result.FirstOrDefault();
            }
            throw new RoomNotFoundException($"Room {id} not found.");
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

        public void RemoveRoom(int id)
        {
            _roomCollection.DeleteOne(x => x.Id == id);
        }

        public Room AddThermostat(int roomId, int thermostatId)
        {
            //Check if room exists
            var roomResult = _roomCollection.Find(r => r.Id == roomId);
            if (roomResult == null)
            {
                throw new RoomNotFoundException($"Room {roomId} not found");
            }
            var room = roomResult.FirstOrDefault();

            //Check if thermostat exists
            var thermostatResult = _thermostatCollection.Find(t => t.Id == thermostatId);
            if (thermostatResult == null)
            {
                throw new ThermostatNotFoundException($"Thermostat {thermostatId} not found");
            }


            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            UpdateDefinition<Room> update = Builders<Room>.Update.AddToSet(r => r.ThermostatIds, thermostatId);
            var updateResult = _roomCollection.UpdateOne(filter, update);
            room.ThermostatIds.Add(thermostatId);
            if (updateResult.IsAcknowledged)
                return room;
            else
                throw new RoomNotFoundException($"Room {room.Id} not found.");
        }


        public Room SetRoomTemperature(int roomId, float temperature)
        {
            var result = _roomCollection.Find(t => t.Id == roomId);

            if (result == null || !result.Any())
            {
                throw new RoomNotFoundException($"Room {roomId} not found.");
            }

            var room = result.FirstOrDefault();

            //Update individual thermostat temperatures by calling their API
            foreach (var id in room.ThermostatIds)
            {
                var thermostat = _thermostatCollection.Find(t => t.Id == id).FirstOrDefault();
                ThermostatClient client = null;
                if (thermostat == null) 
                {
                    //TODO log or throw exception
                    continue;
                }
                client = new ThermostatClient(thermostat.URL);
                thermostat.Temperature = temperature;
                client.UpdateThermostat(thermostat);
            }

            //Update and return room
            room.Temperature = temperature;
            FilterDefinition<Room> filter = Builders<Room>.Filter.Eq(r => r.Id, roomId);
            UpdateDefinition<Room> update = Builders<Room>.Update.Set("Temperature", temperature);
            _roomCollection.UpdateOne(filter, update);
            return room;
        }
    }
}
