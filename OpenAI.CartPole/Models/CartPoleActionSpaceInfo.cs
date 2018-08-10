using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.CartPole.Models
{
    public class CartPoleActionSpaceInfo
    {
        // {
        //     "info": {
        //         "n":2,
        //         "name":"Discrete"
        //     }
        // }


        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

    }
}