using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MainUnit.Models
{
    public class RoomTempMetadataEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public List<int> ThermostatIds { get; set; } = [];
    }

}
