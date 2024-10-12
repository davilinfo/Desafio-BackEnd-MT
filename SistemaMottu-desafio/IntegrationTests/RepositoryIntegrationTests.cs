using Application.Services;
using AutoMapper;
using Domain.Contract;
using Domain.Contract.Mongo;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Persistence.MongoRepository;
using Persistence.Repository;

namespace IntegrationTests
{
    [TestClass]
    public class RepositoryIntegrationTests
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryMotocycleBike _repositoryMotocycleBike;
        private readonly IRepositoryLease _repositoryLease;
        private readonly IRepositoryDeliver _repositoryDeliver;
        private readonly IRepositoryMongoMoto _repositoryMongoMoto;
        private readonly IRepositoryMongoLease _repositoryMongoLease;
        private readonly IRepositoryMongoDeliver _repositoryMongoDeliver;

        public RepositoryIntegrationTests() 
        {             
            _configuration = GetTestConfiguration();
            _repositoryMotocycleBike = new RepositoryMotocycleBike(_configuration);
            _repositoryMongoMoto = new RepositoryMongoMotocycleBike(_configuration);
            _repositoryMongoLease = new RepositoryMongoLease(_configuration);
            _repositoryLease = new RepositoryLease(_configuration);
            _repositoryDeliver = new RepositoryDeliver(_configuration);
            _repositoryMongoDeliver = new RepositoryMongoDeliver(_configuration);
        }
        private static IConfigurationRoot GetTestConfiguration()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName).FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new ConfigurationBuilder()
                .SetBasePath(directory)
                .AddJsonFile(@"appsettings.json")
                .Build();
        }

        #region MotoRepository
        [TestMethod]
        public void RepositoryMoto_GetAll_Test()
        {
            var list = _repositoryMotocycleBike.GetAll().ToList();

            Assert.IsNotNull(list);                  
        }

        [TestMethod]
        public void RepositoryMoto_GetAll_ReturnType_Test()
        {
            var list = _repositoryMotocycleBike.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<MotocycleBike>));
        }

        [TestMethod]
        public void RepositoryMongoMoto_GetAll_ReturnType_Test()
        {
            var list = _repositoryMongoMoto.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<MotocycleBike>));
        }

        [TestMethod]
        public void RepositoryMongoMoto_AddOne_Test()
        {
            Random random = new Random();
            int randomNumber = random.Next();

            var moto = new MotocycleBike($"moto{randomNumber}", 2000, "anyModel", randomNumber.ToString());
            var id = _repositoryMongoMoto.Add(moto).GetAwaiter().GetResult();

            Assert.IsNotNull(id);
            
            var result = _repositoryMongoMoto.GetById(id).GetAwaiter().GetResult();
            Assert.IsTrue(id == result.Identifier);
        }
        #endregion
        #region LeaseRepository
        [TestMethod]
        public void RepositoryLease_GetAll_ReturnType_Test()
        {
            var list = _repositoryLease.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<Lease>));
        }

        [TestMethod]
        public void RepositoryMongoLease_GetAll_ReturnType_Test()
        {
            var list = _repositoryLease.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<Lease>));
        }
        #endregion
        #region DeliverRepository
        [TestMethod]
        public void RepositoryDeliver_GetAll_ReturnType_Test()
        {
            var list = _repositoryDeliver.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<Deliver>));
        }

        [TestMethod]
        public void RepositoryMongoDeliver_GetAll_ReturnType_Test()
        {
            var list = _repositoryMongoDeliver.GetAll().ToList();

            Assert.IsNotNull(list);
            Assert.IsInstanceOfType(list, typeof(List<Deliver>));
        }        
        #endregion
    }
}