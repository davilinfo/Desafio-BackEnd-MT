using System.Text.Json.Serialization;

namespace Application.Models.ViewModel
{
    public class MessageMoto
    {
#pragma warning disable CS8618
        public string Action { get; set; }
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        [JsonPropertyName("modelo")]
        public string Model { get; set; }
        [JsonPropertyName("placa")]
        public string Plate { get; set; }
#pragma warning restore CS8618
    }
}
