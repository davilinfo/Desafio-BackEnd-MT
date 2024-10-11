using Domain.MotocycleBike.Validations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.MotocycleBike.Commands
{
    [ExcludeFromCodeCoverage]
    public class UpdateMotocycleBikeCommand : MotocycleBikeCommand
    {
        public UpdateMotocycleBikeCommand(string identifier, string plate) {
            this.Identifier = identifier;
            this.Plate = plate;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateMotocycleBikeValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Entities.MotocycleBike UpdateMotocycleBike(Entities.MotocycleBike motocycleBike, string plate)
        {            
            return motocycleBike.Update(plate);
        }
    }
}
