using Domain.Deliver.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Validations
{
    [ExcludeFromCodeCoverage]
    internal class UpdateDeliverValidation : DeliverValidation<UpdateDeliverCommand>
    {
        public UpdateDeliverValidation() {
            ValidateDeliverIdentifier();
            ValidateDeliverDriverLicenseImageS3();
        }
    }
}
