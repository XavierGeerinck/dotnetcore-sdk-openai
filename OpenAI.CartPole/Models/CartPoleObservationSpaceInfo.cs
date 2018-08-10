using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.CartPole.Models
{
    public class CartPoleObservationSpaceInfo
    {
        // {
        //     "info": {
        //         "high": [4.7999999999999998, 3.4028234663852886e+38, 0.41887902047863906, 3.4028234663852886e+38],
        //         "low": [-4.7999999999999998, -3.4028234663852886e+38, -0.41887902047863906, -3.4028234663852886e+38],
        //         "name": "Box",
        //         "shape": [4]
        //     }
        // }


        [JsonProperty("high")]
        public double[] High { get; set; }

        [JsonProperty("low")]
        public double[] Low { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shape")]
        public int[] Shape { get; set; }
    }
}