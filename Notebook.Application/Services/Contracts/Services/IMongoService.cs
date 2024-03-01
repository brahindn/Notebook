
using MongoDB.Bson;

namespace Notebook.Application.Services.Contracts.Services
{
    public interface IMongoService
    {
        Task<List<BsonDocument>> GetDataFromMongoDB();
    }
}
