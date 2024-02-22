using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MainUnit.Models.RoomTemperature
{
    public class RoomTemperatureEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public RoomTempMetadataEntry Metadata { get; set; } = new RoomTempMetadataEntry();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public float Temperature { get; set; }
    }
}
