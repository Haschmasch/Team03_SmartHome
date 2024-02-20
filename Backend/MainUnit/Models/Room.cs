using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MainUnit.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public List<int> ThermostatIds { get; set; } = [];

        public float Temperature { get; set; }
    }
}
