using Domain.MotocycleBike.Commands;
using System.Diagnostics.CodeAnalysis;

namespace Domain.MotocycleBike.Validations
{
    [ExcludeFromCodeCoverage]
    internal class UpdateMotocycleBikeValidation : MotocycleBikeValidation<UpdateMotocycleBikeCommand>
    {
        public UpdateMotocycleBikeValidation() {
            ValidateMotocycleBikeIdentifier();
            ValidateMotocycleBikePlate();
        }
    }
}
