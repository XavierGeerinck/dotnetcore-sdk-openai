using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Response
{
    public class EnvResetResponse<T>
    {
        [JsonProperty("observation")]
        public T Observation { get; set; }
    }
}