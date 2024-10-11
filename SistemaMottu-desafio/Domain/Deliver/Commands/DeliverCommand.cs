using System.Diagnostics.CodeAnalysis;

namespace Domain.Deliver.Commands
{
    [ExcludeFromCodeCoverage]
    public abstract class DeliverCommand : Domain.Command.Command
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public string Identifier { get; protected set; }
        public string Name { get; protected set; }
        public string UniqueIdentifier { get; protected set; }        
        public DateTime Birthday { get; protected set; }        
        public string DriverLicenseNumber { get; protected set; }        
        public string DriverLicenseType { get; protected set; }        
        public string DriverLicenseImageS3 { get; protected set; }        
        public string DriverLicenseImage { get; protected set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public DateTime? CreatedDate { get; set; }
    }
}
