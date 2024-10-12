using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Mongo
{
    [ExcludeFromCodeCoverage]
    [Table("locacao")]
    public class LeaseMongo : StringIdentifierIdentityEntity
    {
#pragma warning disable CS8618
        public string _id { get; set; }
        [Column("entregador_id")]
        public string DeliverId { get; set; }
        [Column("moto_id")]
        public string MotocycleBikeId { get; set; }
#pragma warning restore CS8618
        [Column("data_inicio")]
        public DateTime InitialDate { get; set; }
        [Column("data_termino")]
        public DateTime EndDate { get; set; }
        [Column("data_previsao_termino")]
        public DateTime PreviewEndDate { get; set; }
        [Column("plano")]
        public int Plan { get; set; }
        [Column("data_devolucao")]
        public DateTime? DevolutionDate { get; set; }
        [Column("valor_diaria")]
        public double? Value { get; set; }        
    }
}
