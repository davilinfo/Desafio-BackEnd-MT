using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class RequestMotocyclePlate
    {        
        [JsonPropertyName("placa")]
        public string? placa { get;set; }
    }
}
