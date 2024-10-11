using Domain.MotocycleBike.Commands;
using FluentValidation;

namespace Domain.MotocycleBike.Validations
{
    public class MotocycleBikeValidation<T> : AbstractValidator<MotocycleBikeCommand> where T : MotocycleBikeCommand
    {
        const string _mandatoryIdentifier = "O identificador da moto é obrigatório";
        const string _mandatoryYear = "O ano da moto é obrigatório";
        const string _mandatoryModel = "O modelo da moto é obrigatório";
        const string _mandatoryPlate = "A placa da moto é obrigatório";

        public void ValidateMotocycleBikeIdentifier()
        {
            RuleFor(p => p.Identifier).NotEmpty().WithMessage(_mandatoryIdentifier);
        }
        public void ValidateMotocycleBikeYear()
        {
            RuleFor(p=> p.Year).NotEmpty().WithMessage(_mandatoryYear);
        }
        public void ValidateMotocycleBikeModel()
        {
            RuleFor(p=> p.Model).NotEmpty().WithMessage(_mandatoryModel);
        }
        public void ValidateMotocycleBikePlate()
        {
            RuleFor(p=> p.Plate).NotEmpty().WithMessage(_mandatoryPlate);
        }
    }
}
