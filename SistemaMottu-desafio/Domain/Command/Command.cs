using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Command
{
    [ExcludeFromCodeCoverage]
    public abstract class Command
    {
        public DateTime Created { get; private set; }
        public ValidationResult ValidationResult { get; set; }
#pragma warning disable CS8618 
        protected Command() { 
            Created = DateTime.UtcNow.Date;
        }
#pragma warning restore CS8618 

        public abstract bool IsValid();
    }
}
