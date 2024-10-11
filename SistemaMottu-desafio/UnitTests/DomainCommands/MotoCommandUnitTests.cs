using Domain.Entities;
using Domain.MotocycleBike.Commands;

namespace UnitTests.DomainCommands
{
    [TestClass]
    public class MotoCommandUnitTests
    {
        public MotoCommandUnitTests() 
        {
            
        }

        [TestMethod]
        public void RegisterMoto_ShouldSucceed()
        {            
            var plate = "CDX-0101";
            var model = "Mottu Sport";
            var identifier = "moto123";
            var year = 2020;

            
            var registerMoto = new RegisterMotocycleBikeCommand(identifier, year, model, plate);
            var result = registerMoto.IsValid();

            Assert.IsInstanceOfType(registerMoto, typeof(RegisterMotocycleBikeCommand));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegisterMoto_ShouldFail()
        {            
            string plate = "";
            var model = "Mottu Sport";
            var identifier = "moto123";
            var year = 2020;


            var registerMoto = new RegisterMotocycleBikeCommand(identifier, year, model, plate);
            var result = registerMoto.IsValid();

            Assert.IsInstanceOfType(registerMoto, typeof(RegisterMotocycleBikeCommand));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CreateEntityMoto_ShouldSucceed()
        {
            var plate = "CDX-0101";
            var model = "Mottu Sport";
            var identifier = "moto123";
            var year = 2020;

            var registerMoto = new RegisterMotocycleBikeCommand(identifier, year, model, plate);
            var result = registerMoto.IsValid();
            var entity = registerMoto.CreateMotocycleBike();

            Assert.IsTrue(result);
            Assert.IsInstanceOfType(entity, typeof(MotocycleBike));            
        }
    }
}