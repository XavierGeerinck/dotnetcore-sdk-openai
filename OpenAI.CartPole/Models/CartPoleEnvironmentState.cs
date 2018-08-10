using OpenAI.CartPole.Interfaces;

namespace OpenAI.CartPole.Models
{
    public class CartPoleEnvironmentState : IEnvironmentState
    {
        public bool IsDone { get; set; }
        public double Reward { get; set; }
        public double[] Observation { get; set; }
    }
}