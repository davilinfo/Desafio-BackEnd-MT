using Domain.Contract.Mongo;
using Domain.Entities;
using Domain.Entities.Mongo;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Persistence.MongoRepository
{
    public class RepositoryMongoMotocycleBike : IRepositoryMongoMoto
    {
        private MongoContext _mongoContext;
        private readonly string _collection = "moto";
        private readonly string _identifier = "Identifier";
        public RepositoryMongoMotocycleBike(IConfiguration configuration)
        {
            _mongoContext = new MongoContext(configuration);
        }
        public async Task<string> Add(MotocycleBike entity)
        {
            var collection = _mongoContext._database.GetCollection<MotocycleBike>(_collection);
            await collection.InsertOneAsync(entity);
            return entity.Identifier;
        }        

        public async Task<int> Delete(string identifier)
        {
            var filter = Builders<MotocycleBike>.Filter.Eq(_identifier, identifier);
            var collection = _mongoContext._database.GetCollection<MotocycleBike>(_collection);            
            var result = await collection.DeleteOneAsync(filter);
            return int.Parse(result.DeletedCount.ToString());
        }

        public IQueryable<MotocycleBike> GetAll()
        {
            var collection = _mongoContext._database.GetCollection<MotocycleBikeMongo>(_collection);
            var query = from item in collection.AsQueryable<MotocycleBikeMongo>()
                        select new MotocycleBike(item.Identifier, item.Year, item.Model, item.Plate);
            return query;
        }        

#pragma warning disable CS1998 
        public async Task<MotocycleBike> GetById(string identifier)
#pragma warning restore CS1998 
        {            
            var collection = _mongoContext._database.GetCollection<MotocycleBikeMongo>(_collection);
            var result = (from item in collection.AsQueryable<MotocycleBikeMongo>()
                       where item.Identifier == identifier
                       select new MotocycleBike(item.Identifier, item.Year, item.Model, item.Plate)).FirstOrDefault();
#pragma warning disable CS8603            
            return result;
        }
#pragma warning restore CS8603

        public async Task<int> Update(MotocycleBike entity)
        {
            var filter = Builders<MotocycleBike>.Filter.Eq(_identifier, entity.Identifier);
            var collection = _mongoContext._database.GetCollection<MotocycleBike>(_collection);
            var update = Builders<MotocycleBike>.Update.Set("Plate", entity.Plate);
            var result = await collection.UpdateOneAsync(filter,update);

            if (result.ModifiedCount > 0)
            {
                return int.Parse(result.ModifiedCount.ToString());
            }
            return 0;
        }
    }
}
