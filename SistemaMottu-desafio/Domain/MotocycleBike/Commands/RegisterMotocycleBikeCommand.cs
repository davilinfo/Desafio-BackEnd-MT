using Domain.MotocycleBike.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.MotocycleBike.Commands
{
    [ExcludeFromCodeCoverage]
    public class RegisterMotocycleBikeCommand : MotocycleBikeCommand
    {
        public RegisterMotocycleBikeCommand(string identifier, int year, string model, string plate) { 
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }
        public override bool IsValid()
        {
            ValidationResult = new RegisterMotocycleBikeValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Entities.MotocycleBike CreateMotocycleBike()
        {
            return new Entities.MotocycleBike(Identifier, Year, Model, Plate);
        }
    }
}
