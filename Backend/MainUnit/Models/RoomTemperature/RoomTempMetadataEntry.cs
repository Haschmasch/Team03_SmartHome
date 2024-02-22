using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MainUnit.Models.RoomTemperature
{
    public class RoomTempMetadataEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public List<string> ThermostatIds { get; set; } = [];
    }

}
