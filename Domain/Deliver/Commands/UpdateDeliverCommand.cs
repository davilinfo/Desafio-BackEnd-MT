using Domain.Deliver.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Commands
{
    [ExcludeFromCodeCoverage]
    public class UpdateDeliverCommand : DeliverCommand
    {
        public UpdateDeliverCommand(string identifier, string driverLicenseImage) { 
            this.Identifier = identifier;
            this.DriverLicenseImage = driverLicenseImage;            
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateDeliverValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Domain.Entities.Deliver UpdateDeliver(Entities.Deliver deliver, string diverLicenseImage, string diverLicenseImageS3)
        {
            return deliver.Update(diverLicenseImage, diverLicenseImageS3);
        }
    }
}
