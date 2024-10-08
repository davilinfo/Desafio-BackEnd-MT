using Domain.Deliver.Commands;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Validations
{
    [ExcludeFromCodeCoverage]
    public class DeliverValidation<T> : AbstractValidator<DeliverCommand> where T : class
    {
        const string _mandatoryIdentifier = "O identificador é obrigatório";
        const string _mandatoryName = "O nome é obrigatório";
        const string _mandatoryUniqueIdentifier = "O cnpj é obrigatório";
        const string _mandatoryBirthday = "A data de nascimento é obrigatório";
        const string _mandatoryDriverLicenseNumber = "O número cnh é obrigatório";
        const string _mandatoryDriverLicenseType = "O tipo de cnh é obrigatório e só pode ser A, B ou A+B";
        const string _mandatoryDriverLicenseImageS3 = "A imagem de cnh é obrigatório";
        
        public void ValidateDeliverIdentifier()
        {
            RuleFor(p => p.Identifier).NotEmpty().WithMessage(_mandatoryIdentifier);
        }
        public void ValidateDeliverName()
        {
            RuleFor(p=> p.Name).NotEmpty().WithMessage(_mandatoryName);
        }
        public void ValidateDeliverUniqueIdentifier()
        {
            RuleFor(p => p.UniqueIdentifier).NotEmpty().WithMessage(_mandatoryUniqueIdentifier);
        }
        public void ValidateDeliverBirthday()
        {
            RuleFor(p => p.Birthday).NotEmpty().WithMessage(_mandatoryBirthday);
        }
        public void ValidateDeliverDriverLicenseNumber()
        {
            RuleFor(p => p.DriverLicenseNumber).NotEmpty().WithMessage(_mandatoryDriverLicenseNumber);
        }
        public void ValidateDeliverDriverLicenseType()
        {
            RuleFor(p => p.DriverLicenseType).NotEmpty().WithMessage(_mandatoryDriverLicenseType);
            RuleFor(p => p.DriverLicenseType).Must(p=> string.Compare(p, "A", true) == 0 || string.Compare(p, "B", true) == 0 || string.Compare(p, "A+B", true)==0).WithMessage(_mandatoryDriverLicenseType);
        }
        public void ValidateDeliverDriverLicenseImageS3()
        {
            RuleFor(p => p.DriverLicenseImageS3).NotEmpty().WithMessage(_mandatoryDriverLicenseImageS3);
        }
    }
}
