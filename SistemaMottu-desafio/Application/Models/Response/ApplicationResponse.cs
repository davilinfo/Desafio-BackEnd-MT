using Newtonsoft.Json;

namespace Application.Models.Response
{
    public class ApplicationResponse
    {
        [JsonProperty("mensagem")]
        public string Message { get; set; }
        public ApplicationResponse(string message) { 
            Message = message;
        }
    }
}
