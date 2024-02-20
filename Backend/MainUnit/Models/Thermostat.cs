using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MainUnit.Models
{
    public class Thermostat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public float Temperature { get; set; }
        public int RoomId { get; set; }
    }
}
