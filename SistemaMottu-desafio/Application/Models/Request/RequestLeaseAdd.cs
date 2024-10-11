using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestLeaseAdd
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Identificador entregador é obrigatório")]
        [JsonPropertyName("entregador_id")]
        public required string DeliverId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Identificador moto é obrigatório")]
        [JsonPropertyName("moto_id")]
        public required string MotocycleBikeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Data início é obrigatório")]
        [JsonPropertyName("data_inicio")]
        public DateTime InitialDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Data termino é obrigatório")]
        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Data previsão termino é obrigatório")]
        [JsonPropertyName("data_previsao_termino")]
        public DateTime PreviewEndDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Plano é obrigatório")]
        [JsonPropertyName("plano")]
        public int Plan { get; set; }
    }
}
