using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestDeliverUpdate
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Imagem cnh é obrigatório")]
        [JsonPropertyName("imagem_cnh")]
        public required string DriverLicenseImage { get; set; }
    }
}
