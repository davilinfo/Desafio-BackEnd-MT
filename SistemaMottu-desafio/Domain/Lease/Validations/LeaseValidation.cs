using Domain.Lease.Commands;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Validations
{
    [ExcludeFromCodeCoverage]
    public class LeaseValidation<T> : AbstractValidator<LeaseCommand> where T : class
    {
        const string _mandatoryRules = "Dados inválidos";
        const string _planRule = "Plano pode ser apenas 7, 15, 30, 45, 50";
        public void ValidateLeaseMotocycleBikeId()
        {
            RuleFor(p=> p.MotocycleBikeId).NotEmpty().WithMessage(_mandatoryRules);
        }
        public void ValidateLeaseDeliverId()
        {
            RuleFor(p=> p.DeliverId).NotEmpty().WithMessage(_mandatoryRules);
        }
        public void ValidateLeaseInitialDate()
        {
            RuleFor(p=> p.InitialDate).NotEmpty().WithMessage(_mandatoryRules);
        }
        public void ValidateLeaseEndDate()
        {
            RuleFor(p=> p.EndDate).NotEmpty().WithMessage(_mandatoryRules);
        }
        public void ValidateLeasePreviewEndDate()
        {
            RuleFor(p => p.PreviewEndDate).NotEmpty().WithMessage(_mandatoryRules);
        }
        public void ValidateLeasePlan()
        {
            RuleFor(p=> p.Plan).NotEmpty().WithMessage(_mandatoryRules);
            RuleFor(p => p.Plan).Must(p => p == 7 || p == 15 || p == 30 || p == 45 || p == 50).WithMessage(_planRule);
        }
        public void ValidateLeaseDevolutionDate()
        {
            RuleFor(p=>p.DevolutionDate).NotEmpty().WithMessage(_mandatoryRules);
        }
    }
}
