using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AdminWebApp.Server.Models.MongoDB
{
    public class EquationDoc
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("equation")]
        public string Equation { get; set; }

    }
}
