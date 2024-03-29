﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MainUnit.Models.Room
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(BaseRoom))]
    public class BaseRoom
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
