using Domain.Contract.Mongo;
using Domain.Entities;
using Domain.Entities.Mongo;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Persistence.MongoRepository
{
    public class RepositoryMongoDeliver : IRepositoryMongoDeliver
    {
        private MongoContext _mongoContext;
        private readonly string _collection = "entregador";
        private readonly string _identifier = "Identifier";
        public RepositoryMongoDeliver(IConfiguration configuration)
        {
            _mongoContext = new MongoContext(configuration);
        }
        public async Task<string> Add(Deliver entity)
        {
            var collection = _mongoContext._database.GetCollection<Deliver>(_collection);
            await collection.InsertOneAsync(entity);
            return entity.Identifier;
        }        

        public async Task<int> Delete(string identifier)
        {
            var filter = Builders<Deliver>.Filter.Eq(_identifier, identifier);
            var collection = _mongoContext._database.GetCollection<Deliver>(_collection);            
            var result = await collection.DeleteOneAsync(filter);
            return int.Parse(result.DeletedCount.ToString());            
        }

        public IQueryable<Deliver> GetAll()
        {
            var collection = _mongoContext._database.GetCollection<DeliverMongo>(_collection);
            var query = from item in collection.AsQueryable<DeliverMongo>()
                        select new Deliver { 
                            Identifier = item.Identifier,
                            Birthday = item.Birthday,
                            CreatedDate = item.CreatedDate,
                            DriverLicenseImageS3 = item.DriverLicenseImage,
                            DriverLicenseNumber = item.DriverLicenseNumber,
                            DriverLicenseType = item.DriverLicenseType,
                            Name = item.Name,
                            UniqueIdentifier = item.UniqueIdentifier
                        };
            return query;
        }

#pragma warning disable CS1998 
        public async Task<Deliver> GetById(string identifier)
#pragma warning restore CS1998 
        {
            var filter = Builders<DeliverMongo>.Filter.Eq(_identifier, identifier);
            var collection = _mongoContext._database.GetCollection<DeliverMongo>(_collection);
            var result = (from item in collection.AsQueryable<DeliverMongo>()
                          where item.Identifier == identifier
                          select new Deliver
                          {
                              Identifier = item.Identifier,
                              Birthday = item.Birthday,
                              CreatedDate = item.CreatedDate,
                              DriverLicenseImageS3 = item.DriverLicenseImage,
                              DriverLicenseNumber = item.DriverLicenseNumber,
                              DriverLicenseType = item.DriverLicenseType,
                              Name = item.Name,
                              UniqueIdentifier = item.UniqueIdentifier
                          }).FirstOrDefault();

#pragma warning disable CS8603
            return result;
#pragma warning restore CS8603
        }

        public async Task<int> Update(Deliver entity)
        {
            var filter = Builders<Deliver>.Filter.Eq(_identifier, entity.Identifier);
            var collection = _mongoContext._database.GetCollection<Deliver>(_collection);
            var update = Builders<Deliver>.Update.Set("DriverLicenseImageS3", entity.DriverLicenseImageS3);
            var result = await collection.UpdateOneAsync(filter,update);

            if (result.ModifiedCount > 0)
            {
                return int.Parse(result.ModifiedCount.ToString());
            }
            return 0;
        }        
    }
}
