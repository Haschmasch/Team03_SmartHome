using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MainUnit.Models.Thermostat
{
    public class Thermostat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public float Temperature { get; set; }
        public string RoomId { get; set; } = null!;
    }
}
