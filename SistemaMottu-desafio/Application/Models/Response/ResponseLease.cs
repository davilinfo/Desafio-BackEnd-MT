using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.Response
{
    public class ResponseLease
    {
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }
        [JsonPropertyName("valor_diaria")]
        public double? Value { get; set; }
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
        [JsonPropertyName("data_devolucao")]
        public DateTime? DevolutionDate { get; set; }        
    }
}
