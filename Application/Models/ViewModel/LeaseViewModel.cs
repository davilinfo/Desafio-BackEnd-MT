using System.Text.Json.Serialization;

namespace Application.Models.ViewModel
{
    public class LeaseViewModel
    {
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }        
        [JsonPropertyName("entregador_id")]
        public required string DeliverId { get; set; }
        [JsonPropertyName("moto_id")]
        public required string MotocycleBikeId { get; set; }
        [JsonPropertyName("data_inicio")]
        public DateTime InitialDate { get; set; }
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("data_previsao_termino")]
        public DateTime PreviewEndDate { get; set; }
        [JsonPropertyName("plano")]
        public int Plan { get; set; }
        [JsonPropertyName("data_devolucao")]
        public DateTime? DevolutionDate { get; set; }
        [JsonPropertyName("valor_diaria")]
        public double? Value { get; set; }
    }
}
