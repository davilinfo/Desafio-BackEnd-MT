using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestMotocycleAdd
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Identificador é obrigatório")]
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ano é obrigatório")]
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Model é obrigatório")]
        [JsonPropertyName("modelo")]
        public required string Model { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Placa é obrigatório")]
        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
