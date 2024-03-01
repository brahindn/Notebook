using Notebook.Application.Services.Contracts.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace Notebook.Application.Services.Implementation.Services
{
    public class MongoService : IMongoService
    {
        private readonly string _connectionString;
        public MongoService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MongoDBconnection");
        }

        public async Task<List<BsonDocument>> GetDataFromMongoDB()
        {
            var instance = new MongoClient(_connectionString);
            var database = instance.GetDatabase("NotebookLogDB");

            var collection = database.GetCollection<BsonDocument>("AppLogs");

            return await collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
