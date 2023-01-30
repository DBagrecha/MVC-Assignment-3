using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Assignment3.Models
{
    public class Image
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string ImName { get; set; }
    }
}
