﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MainUnit.Models.RoomTemperature
{
    public class RoomTempMetadataEntry
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RoomId { get; set; }
        public List<string> ThermostatIds { get; set; } = [];
    }

}
