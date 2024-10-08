using System.Diagnostics.CodeAnalysis;

namespace Domain.Lease.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class LeaseCommand : Domain.Command.Command
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public string Identifier { get; protected set; }
        public string DeliverId { get; protected set; }
        public string MotocycleBikeId { get; protected set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public DateTime InitialDate { get; protected set; }        
        public DateTime EndDate { get; protected set; }        
        public DateTime PreviewEndDate { get; protected set; }
        public int Plan { get; protected set; }        
        public DateTime? DevolutionDate { get; protected set; }       
        public double? Value { get; protected set; }
    }
}
