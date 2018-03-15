using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using pkdotnet.Models;

namespace pkdotnet.DataAccess
{
    public class MongoAccess
    {
		readonly Lazy<MongoClient> _client;
		readonly Lazy<IMongoDatabase> _db;

		public MongoAccess(IConfiguration configuration)
        {
			_client = new Lazy<MongoClient>(() => new MongoClient(configuration["db:connectionString"]));
			_db = new Lazy<IMongoDatabase>(() => _client.Value.GetDatabase(configuration["db:name"]));
        }

        public virtual async Task<List<DataSource>> GetAll()
        {
			var collection = _db.Value.GetCollection<DataSource>("dataSources");

			return await collection.Find(new BsonDocument()).ToListAsync();
        }

		public virtual async Task<DataSource> GetOne(string id)
		{
			var collection = _db.Value.GetCollection<DataSource>("dataSources");

			FilterDefinition<DataSource> filter = new BsonDocument("_id", new ObjectId(id));
			return await collection.Find(filter).FirstOrDefaultAsync();
		}

		public virtual async Task<bool> DeleteOne(string id)
		{
			var collection = _db.Value.GetCollection<DataSource>("dataSources");

			FilterDefinition<DataSource> filter = new BsonDocument("_id", new ObjectId(id));
			var result = await collection.DeleteOneAsync(filter);
			return result.DeletedCount == 1;
		}

		public virtual async Task<DataSource> AddOne(DataSource ds)
		{
			var collection = _db.Value.GetCollection<DataSource>("dataSources");

			await collection.InsertOneAsync(ds);
			return ds;
		}
    }
}
