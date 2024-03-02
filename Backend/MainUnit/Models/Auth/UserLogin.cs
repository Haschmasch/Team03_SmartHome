using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MainUnit.Models.Auth
{
    public class UserLogin
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserLogin(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
