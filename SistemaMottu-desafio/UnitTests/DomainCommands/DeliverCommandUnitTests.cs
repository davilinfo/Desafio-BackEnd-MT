using Domain.Entities;
using Domain.Deliver.Commands;

namespace UnitTests.DomainCommands
{
    [TestClass]
    public class DeliverCommandUnitTests
    {
        [TestMethod]
        public void RegisterDeliver_ShouldSucceed()
        {
            var identifier = "entregador123";
            var name = "entregador";
            var uniqueIdentifier = "cnpj";
            var birthday = new DateTime(2000, 05, 20);
            var driverLicenseNumber = "123";
            var driverLicenseType = "A";
            var driverLicenseImage = "sadasdad";

            var registerDeliver = new RegisterDeliverCommand(identifier,name,uniqueIdentifier, birthday, driverLicenseNumber, driverLicenseType, driverLicenseImage);
            var result = registerDeliver.IsValid();

            Assert.IsInstanceOfType(registerDeliver, typeof(RegisterDeliverCommand));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateDeliverEntity_ShouldSucceed()
        {
            var identifier = "entregador123";
            var name = "entregador";
            var uniqueIdentifier = "cnpj";
            var birthday = new DateTime(2000, 05, 20);
            var driverLicenseNumber = "123";
            var driverLicenseType = "A";
            var driverLicenseImage = "sadasdad";

            var registerDeliver = new RegisterDeliverCommand(identifier, name, uniqueIdentifier, birthday, driverLicenseNumber, driverLicenseType, driverLicenseImage);
            var result = registerDeliver.IsValid();
            var entity = registerDeliver.CreateDeliver();

            Assert.IsInstanceOfType(entity, typeof(Deliver));
            Assert.IsTrue(result);
        }

        #region Mutation test
        [TestMethod]
        public void RegisterDeliver_InvalidCNHType_ShouldFail()
        {
            var identifier = "entregador123";
            var name = "entregador";
            var uniqueIdentifier = "cnpj";
            var birthday = new DateTime(2000, 05, 20);
            var driverLicenseNumber = "123";
            var driverLicenseType = "T";
            var driverLicenseImage = "sadasdad";

            var registerDeliver = new RegisterDeliverCommand(identifier, name, uniqueIdentifier, birthday, driverLicenseNumber, driverLicenseType, driverLicenseImage);
            var result = registerDeliver.IsValid();

            Assert.IsInstanceOfType(registerDeliver, typeof(RegisterDeliverCommand));
            Assert.IsFalse(result);
        }
        #endregion
    }
}
