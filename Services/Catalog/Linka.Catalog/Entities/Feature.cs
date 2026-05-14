using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Linka.Catalog.Entities
{
    public class Feature
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string Icon { get; set; }
    }
}
