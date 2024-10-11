using System.Text.Json.Serialization;

namespace Application.Models.Response
{
    public class ResponseMotocycleBike
    {
        /// <summary>
        /// identificador: "moto123"
        /// </summary>
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }
        /// <summary>
        /// ano: 2020
        /// </summary>
        [JsonPropertyName("ano")]        
        public int Year { get; set; }
        /// <summary>
        /// modelo: "Mottu Sport"
        /// </summary>
        [JsonPropertyName("modelo")]
        public required string Model { get; set; }
        /// <summary>
        /// placa: "CDX-0101"
        /// </summary>
        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
