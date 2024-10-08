using Domain.Lease.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Commands
{
    [ExcludeFromCodeCoverage]
    public class UpdateLeaseCommand : LeaseCommand
    {
        public UpdateLeaseCommand(string identifier, DateTime devolutionDate) { 
            Identifier = identifier;
            DevolutionDate = devolutionDate;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateLeaseValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Entities.Lease UpdateLease(Entities.Lease lease)
        {
            return lease.UpdateLease(DevolutionDate);
        }
    }
}
