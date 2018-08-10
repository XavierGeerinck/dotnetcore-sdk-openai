using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Request
{
    public class EnvStepRequest
    {
        [JsonProperty("action")]
        public int Action { get; set; }

        [JsonProperty("render")]
        public bool IsRender { get; set; }
    }
}