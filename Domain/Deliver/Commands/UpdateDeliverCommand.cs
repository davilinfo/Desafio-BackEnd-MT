using Domain.Deliver.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Commands
{
    [ExcludeFromCodeCoverage]
    public class UpdateDeliverCommand : DeliverCommand
    {
        public UpdateDeliverCommand(string identifier, string driverLicenseImage, string driverLicenseS3) { 
            this.Identifier = identifier;
            this.DriverLicenseImage = driverLicenseImage;
            this.DriverLicenseImageS3 = driverLicenseS3;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateDeliverValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Domain.Entities.Deliver UpdateDeliver(Entities.Deliver deliver)
        {
            return deliver.Update(DriverLicenseImage, DriverLicenseImageS3);
        }
    }
}
