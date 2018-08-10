using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.SDK.Models.Response
{
    public class EnvGetActionSpaceResponse<T>
    {
        [JsonProperty("info")]
        public T Info { get; set; }
    }
}