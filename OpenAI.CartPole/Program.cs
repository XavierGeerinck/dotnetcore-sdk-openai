using System;
using OpenAI.SDK;

namespace OpenAI.CartPole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var sdk = new ApiService();
            var experiment = new CartPoleExperiment(sdk);
            experiment.Initialize().Wait();
            experiment.Run().Wait();
        }
    }
}
