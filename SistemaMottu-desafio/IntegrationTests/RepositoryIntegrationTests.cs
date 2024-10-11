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
        private readonly IRepositoryMongoMoto _repositoryMongoMoto;
        private readonly IMapper _mapper;
        public RepositoryIntegrationTests() 
        {             
            _configuration = GetTestConfiguration();
            _repositoryMotocycleBike = new RepositoryMotocycleBike(_configuration);
            _repositoryMongoMoto = new RepositoryMongoMotocycleBike(_configuration);
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
        public void RepositoryMongoMoto_AddOne_Test()
        {
            Random random = new Random();
            int randomNumber = random.Next();

            var moto = new MotocycleBike($"moto{randomNumber}", 2000, "anyModel", random.ToString());
            var id = _repositoryMongoMoto.Add(moto).GetAwaiter().GetResult();

            Assert.IsNotNull(id);            
        }        
    }
}