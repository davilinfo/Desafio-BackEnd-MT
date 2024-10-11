using Domain.Deliver.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Validations
{
    [ExcludeFromCodeCoverage]
    internal class RegisterDeliverValidation : DeliverValidation<RegisterDeliverCommand>
    {
        public RegisterDeliverValidation() {
            ValidateDeliverIdentifier();
            ValidateDeliverName();
            ValidateDeliverUniqueIdentifier();
            ValidateDeliverBirthday();
            ValidateDeliverDriverLicenseNumber();
            ValidateDeliverDriverLicenseType();
            ValidateDeliverDriverLicenseImage();
        }
    }
}
