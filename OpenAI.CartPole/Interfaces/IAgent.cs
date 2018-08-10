using System;
using System.Threading.Tasks;

namespace OpenAI.CartPole.Interfaces
{
    public interface IAgent
    {
        Task<IEnvironmentState> Act(int episode, double[] observation);
    }
}