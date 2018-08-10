using System;
using System.Threading.Tasks;

namespace OpenAI.CartPole.Interfaces
{
    public interface IExperiment
    {
        Task Initialize();
        Task Run();
    }
}