using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Request
{
    public class EnvMonitorStartRequest
    {
        [JsonProperty("directory")]
        public string Directory { get; set; }

        [JsonProperty("force")]
        public bool Force { get; set; }

        [JsonProperty("resume")]
        public bool Resume { get; set; }
    }
}