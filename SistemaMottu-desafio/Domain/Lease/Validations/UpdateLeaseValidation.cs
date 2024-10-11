using Domain.Lease.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Validations
{
    [ExcludeFromCodeCoverage]
    public class UpdateLeaseValidation : LeaseValidation<UpdateLeaseCommand>
    {
        public UpdateLeaseValidation() { 
            ValidateLeaseDevolutionDate();
        }
    }
}
