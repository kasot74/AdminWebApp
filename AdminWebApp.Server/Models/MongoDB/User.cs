using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AdminWebApp.Server.Models.MongoDB
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("passwordsalt")]
        public string PasswordSalt { get; set; }

        [BsonElement("passwordhash")]
        public string PasswordHash { get; set; }
        
        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("lastLoginAt")]
        public DateTime LastLoginAt { get; set; }
    }
}
