﻿using System.Text.Json.Serialization;

namespace Application.Models.Response
{
    public class ResponseMotocycleBike
    {
        [JsonPropertyName("identificador")]
        public required string Identifier { get; set; }
        [JsonPropertyName("ano")]
        public int Year { get; set; }
        [JsonPropertyName("modelo")]
        public required string Model { get; set; }
        [JsonPropertyName("placa")]
        public required string Plate { get; set; }
    }
}
