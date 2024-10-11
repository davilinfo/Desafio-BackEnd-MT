using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestDeliverAdd
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Identificador é obrigatório")]
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome é obrigatório")]
        [JsonPropertyName("nome")]
        public required string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Número CNPJ é obrigatório")]
        [JsonPropertyName("cnpj")]        
        public required string UniqueIdentifier { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Data nascimento é obrigatório")]
        [JsonPropertyName("data_nascimento")]
        public DateTime Birthday { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Número cnh é obrigatório")]
        [JsonPropertyName("numero_cnh")]
        public required string DriverLicenseNumber { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tipo cnh é obrigatório")]
        [JsonPropertyName("tipo_cnh")]
        public required string DriverLicenseType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Imagem cnh é obrigatório")]
        [JsonPropertyName("imagem_cnh")]
        public required string DriverLicenseImage { get; set; }
    }
}
