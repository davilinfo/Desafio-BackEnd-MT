using Domain.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [ExcludeFromCodeCoverage]
    [Table("locacao")]
    public class Lease : StringIdentifierIdentityEntity
    {
        [Column("entregador_id")]
        public string DeliverId {  get; set; }
        [Column("moto_id")]
        public string MotocycleBikeId {  get; set; }
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
        public double? Value {  get; set; }

        public Lease(string deliverId, string motocycleBikeId, DateTime initialDate, DateTime endDate, DateTime previewEndDate, int plan)
        {
            DeliverId = deliverId;
            MotocycleBikeId = motocycleBikeId;
            InitialDate = initialDate;
            EndDate = endDate;
            PreviewEndDate = previewEndDate;
            Plan = plan;            
        }

        public Lease UpdateLease(DateTime? devolutionDate)
        {
            DevolutionDate = devolutionDate;

            return this;
        }
    }
}
