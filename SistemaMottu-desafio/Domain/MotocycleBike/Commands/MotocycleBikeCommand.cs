namespace Domain.MotocycleBike.Commands
{
    public abstract class MotocycleBikeCommand : Domain.Command.Command
    {
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
        public string Identifier { get; protected set; }
        public int Year { get; protected set; }
        public string Model { get; protected set; }
        public string Plate { get; protected set; }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere adicionar o modificador "obrigatório" ou declarar como anulável.
    }
}
