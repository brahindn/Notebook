using Notebook.Application.Services.Contracts.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Notebook.Application.Services.Implementation.Services
{
    public class MongoService : IMongoService
    {
        public async Task<List<BsonDocument>> GetDataFromMongoDB()
        {
            var instance = new MongoClient("mongodb://localhost:27017/NotebookLogDB");
            var database = instance.GetDatabase("NotebookLogDB");

            var collection = database.GetCollection<BsonDocument>("AppLogs");

            return await collection.Find(new BsonDocument()).ToListAsync();
        }
    }
}
