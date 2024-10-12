using Domain.Entities;
using Domain.Lease.Commands;

namespace UnitTests.DomainCommands
{
    [TestClass]
    public class LeaseCommandUnitTests
    {
        [TestMethod]
        public void RegisterLease_ShouldSucceed()
        {            
            var motoId = "moto123";
            var deliverId = "entregador123";            
            var initialDate = new DateTime(2024, 07, 01);
            var endDate = new DateTime(2024, 07, 07);
            var previewEndDate = new DateTime(2024, 07, 07);
            var plan = 7;            

            var registerLease = new RegisterLeaseCommand(deliverId, motoId, initialDate, endDate, previewEndDate, plan);
            var result = registerLease.IsValid();

            Assert.IsInstanceOfType(registerLease, typeof(RegisterLeaseCommand));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateDeliverEntity_ShouldSucceed()
        {
            var motoId = "moto123";
            var deliverId = "entregador123";
            var initialDate = new DateTime(2024, 07, 01);
            var endDate = new DateTime(2024, 07, 07);
            var previewEndDate = new DateTime(2024, 07, 07);
            var plan = 7;

            var registerLease = new RegisterLeaseCommand(deliverId, motoId, initialDate, endDate, previewEndDate, plan);
            var result = registerLease.IsValid();

            var entity = registerLease.CreateLease();

            Assert.IsInstanceOfType(entity, typeof(Lease));            
            Assert.IsTrue(result);
        }

        #region Mutation test
        [TestMethod]
        public void RegisterLease_InvalidPlan_ShouldFail()
        {
            var motoId = "moto123";
            var deliverId = "entregador123";
            var initialDate = new DateTime(2024, 07, 01);
            var endDate = new DateTime(2024, 07, 07);
            var previewEndDate = new DateTime(2024, 07, 07);
            var plan = 90;

            var registerLease = new RegisterLeaseCommand(deliverId, motoId, initialDate, endDate, previewEndDate, plan);
            var result = registerLease.IsValid();

            Assert.IsInstanceOfType(registerLease, typeof(RegisterLeaseCommand));
            Assert.IsFalse(result);
        }
        #endregion
    }
}
