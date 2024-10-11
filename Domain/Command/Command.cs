using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Command
{
    [ExcludeFromCodeCoverage]
    public abstract class Command
    {
        public DateTime Created { get; private set; }
        public required ValidationResult ValidationResult { get; set; }
        protected Command() { 
            Created = DateTime.UtcNow.Date;
        }

        public abstract bool IsValid();
    }
}
