using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Request
{
    public class EnvCreateRequest
    {
        [JsonProperty("env_id")]
        public string EnvId { get; set; }
    }
}