using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Response
{
    public class EnvCreateResponse
    {
        [JsonProperty("instance_id")]
        public string InstanceID { get; set; }
    }
}