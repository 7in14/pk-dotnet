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
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;

        public MongoAccess(IConfiguration configuration)
        {
            _client = new MongoClient(configuration["db:connectionString"]);
            _db = _client.GetDatabase(configuration["db:name"]);
        }

        public virtual async Task<List<DataSource>> GetAll()
        {
            List<DataSource> result = new List<DataSource>();

            using (var cursor = await _db.GetCollection<DataSource>("dataSources").FindAsync(new BsonDocument()))
            {

                while (await cursor.MoveNextAsync())
                {

                    var batch = cursor.Current;
                    result.AddRange(batch);
                }
            }

            return result;
        }
    }
}
