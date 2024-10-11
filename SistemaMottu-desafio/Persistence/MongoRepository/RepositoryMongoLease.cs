using Domain.Contract.Mongo;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Persistence.MongoRepository
{
    public class RepositoryMongoLease : IRepositoryMongoLease
    {
        private MongoContext _mongoContext;
        private readonly string _collection = "locacao";
        private readonly string _identifier = "Identifier";
        public RepositoryMongoLease(IConfiguration configuration)
        {
            _mongoContext = new MongoContext(configuration);
        }
        public async Task<string> Add(Lease entity)
        {
            var collection = _mongoContext._database.GetCollection<Lease>(_collection);
            await collection.InsertOneAsync(entity);
            return entity.Identifier;
        }        

        public async Task<int> Delete(string identifier)
        {
            var filter = Builders<Lease>.Filter.Eq(_identifier, identifier);
            var collection = _mongoContext._database.GetCollection<Lease>(_collection);           
            var result = await collection.DeleteOneAsync(filter);
            return int.Parse(result.DeletedCount.ToString());            
        }

        public IQueryable<Lease> GetAll()
        {
            var collection = _mongoContext._database.GetCollection<Lease>(_collection);
            return collection.AsQueryable();
        }

        public async Task<Lease> GetById(string identifier)
        {
            var filter = Builders<Lease>.Filter.Eq(_identifier, identifier);
            var collection = _mongoContext._database.GetCollection<Lease>(_collection);
            var item = collection.AsQueryable().FirstOrDefault(p=> p.Identifier == identifier);

#pragma warning disable CS8603
            return item;
#pragma warning restore CS8603
        }

        public async Task<int> Update(Lease entity)
        {
            var filter = Builders<Lease>.Filter.Eq(_identifier, entity.Identifier);
            var collection = _mongoContext._database.GetCollection<Lease>(_collection);
            var update = Builders<Lease>.Update.Set("DevolutionDate", entity.DevolutionDate);            
            var result = await collection.UpdateOneAsync(filter,update);
            var update2 = Builders<Lease>.Update.Set("Value", entity.Value);
            result = await collection.UpdateOneAsync(filter, update2);

            if (result.ModifiedCount > 0)
            {
                return int.Parse(result.ModifiedCount.ToString());
            }
            return 0;
        }        
    }
}
