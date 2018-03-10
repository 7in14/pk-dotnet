using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pkdotnet.Models
{
    public class DataSource
	{
		[BsonIgnoreIfNull]
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }
    }
}
