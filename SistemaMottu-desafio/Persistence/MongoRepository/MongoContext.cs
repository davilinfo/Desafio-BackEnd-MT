using Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Persistence
{
    public class MongoContext
	{

		private readonly IConfiguration _configuration;
		public readonly IMongoDatabase _database;
		public MongoContext(IConfiguration configuration)
		{
            _configuration = configuration;
			
            var client = new MongoClient(_configuration.GetConnectionString("Mongo"));
			_database = client.GetDatabase("moto");			
			VerifyCollections();
		}

		private void VerifyCollections()
		{
			var motoCollection = _database.GetCollection<MotocycleBike>("moto");
			if (motoCollection == null)
			{
				_database.CreateCollection("moto");
			}
            var deliverCollection = _database.GetCollection<Deliver>("deliver");
            if (deliverCollection == null)
            {
                _database.CreateCollection("deliver");
            }
            var locacaoCollection = _database.GetCollection<Lease>("locacao");
            if (locacaoCollection == null)
            {
                _database.CreateCollection("locacao");
            }
        }
	}
}
