using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Response
{
    public class EnvStepResponse<T>
    {
        [JsonProperty("done")]
        public bool IsDone { get; set; }

        [JsonProperty("info")]
        public Dictionary<string, string> Info { get; set; }

        [JsonProperty("observation")]
        public T Observation { get; set; }

        [JsonProperty("reward")]
        public double Reward { get; set; }
    }
}