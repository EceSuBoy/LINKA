using MongoDB.Bson.Serialization.Attributes;

namespace Linka.Catalog.Entities
{
    public class ProductImage
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ProductImageId { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string ProductId { get; set; }

        [BsonIgnore]
        public Product Product { get; set; }
    }
}
