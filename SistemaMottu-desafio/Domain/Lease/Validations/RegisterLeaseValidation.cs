using Domain.Lease.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Validations
{
    [ExcludeFromCodeCoverage]
    public class RegisterLeaseValidation : LeaseValidation<RegisterLeaseCommand>
    {
        public RegisterLeaseValidation() { 
            ValidateLeaseDeliverId();
            ValidateLeaseMotocycleBikeId();
            ValidateLeaseInitialDate();
            ValidateLeaseEndDate();
            ValidateLeasePreviewEndDate();
            ValidateLeasePlan();
        }
    }
}
    