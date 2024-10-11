using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestLeaseUpdate
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Data devolução é obrigatório")]
        [JsonPropertyName("data_devolucao")]
        public DateTime DevolutionDate { get; set; }
    }
}
