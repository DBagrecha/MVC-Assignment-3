using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Assignment3.Models
{
    public class Caption
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string desc { get; set; }
    }
}
