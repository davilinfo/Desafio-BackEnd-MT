using Domain.Lease.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Commands
{
    [ExcludeFromCodeCoverage]
    public class RegisterLeaseCommand : LeaseCommand
    {
        public RegisterLeaseCommand(string deliverId, string motocycleBikeId, DateTime initialDate, DateTime endDate, DateTime previewEndDate, int plan) {
            DeliverId = deliverId;
            MotocycleBikeId = motocycleBikeId;
            InitialDate = initialDate;
            EndDate = endDate;
            PreviewEndDate = previewEndDate;
            Plan = plan;
        }
        public override bool IsValid()
        {
            ValidationResult = new RegisterLeaseValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Entities.Lease CreateLease()
        {
            return new Entities.Lease(DeliverId, MotocycleBikeId, InitialDate, EndDate, PreviewEndDate, Plan);
        }
    }
}
