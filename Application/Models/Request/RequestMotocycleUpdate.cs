using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestMotocycleUpdate
    {                
        [Required(AllowEmptyStrings = false, ErrorMessage = "Placa é obrigatório")]
        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
