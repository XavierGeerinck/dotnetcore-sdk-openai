using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenAI.SDK.Models.Response
{
    public class EnvGetObservationSpaceResponse<T>
    {
        [JsonProperty("info")]
        public T Info { get; set; }
    }
}