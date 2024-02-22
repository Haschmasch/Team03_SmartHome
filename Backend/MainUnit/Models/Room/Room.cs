using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MainUnit.Models.Room
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public List<string> ThermostatIds { get; set; } = [];

        public float Temperature { get; set; }
    }
}
