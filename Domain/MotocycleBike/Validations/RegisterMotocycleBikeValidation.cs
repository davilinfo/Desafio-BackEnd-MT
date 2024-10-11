using Domain.MotocycleBike.Commands;

namespace Domain.MotocycleBike.Validations
{
    internal class RegisterMotocycleBikeValidation : MotocycleBikeValidation<RegisterMotocycleBikeCommand>
    {
        public RegisterMotocycleBikeValidation() { 
            ValidateMotocycleBikeIdentifier();
            ValidateMotocycleBikeModel();
            ValidateMotocycleBikePlate();
            ValidateMotocycleBikeYear();
        }
    }
}
