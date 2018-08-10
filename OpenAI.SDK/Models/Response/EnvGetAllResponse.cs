using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Response
{
    public class EnvGetAllResponse
    {
        [JsonProperty("all_envs")]
        public Dictionary<string, string> Environments { get; set; }
    }
}