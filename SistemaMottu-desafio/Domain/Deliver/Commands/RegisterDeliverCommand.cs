﻿using Domain.Deliver.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Commands
{
    [ExcludeFromCodeCoverage]
    public class RegisterDeliverCommand : DeliverCommand
    {
        public RegisterDeliverCommand(
            string identifier,
            string name,
            string uniqueIdentifier,
            DateTime birthday,
            string driverLicenseNumber,
            string driverLicenseType,            
            string driverLicenseImage)
        {
            this.Identifier = identifier;
            this.Name = name;
            this.Birthday = birthday;
            this.DriverLicenseNumber = driverLicenseNumber;
            this.DriverLicenseImage = driverLicenseImage;
            this.DriverLicenseType = driverLicenseType;
            this.UniqueIdentifier = uniqueIdentifier;            
            this.DriverLicenseImage = driverLicenseImage;
        }

        public Domain.Entities.Deliver CreateDeliver()
        {
            return new Entities.Deliver(Identifier, Name, UniqueIdentifier, Birthday, DriverLicenseNumber, DriverLicenseType, DriverLicenseImage);
        }        

        public override bool IsValid()
        {
            ValidationResult = new RegisterDeliverValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
