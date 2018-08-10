using System;
using System.Threading.Tasks;
using OpenAI.SDK;
using OpenAI.SDK.Models.Response;
using OpenAI.CartPole.Models;
using OpenAI.CartPole.Interfaces;
using System.Diagnostics;

namespace OpenAI.CartPole
{
    public class CartPoleExperiment : IExperiment
    {
        private const int EPISODE_COUNT = 1000;
        private const int MAX_TIME_STEPS = 250;

        private readonly ApiService client;
        private CartPoleAgent agent;
        private EnvCreateResponse environment;

        public CartPoleExperiment(ApiService client)
        {
            this.client = client;
        }

        public async Task Initialize()
        {
            this.environment = await this.client.EnvCreate("CartPole-v0");
            var actionSpaceInfo = await this.client.EnvActionSpaceInfo<CartPoleActionSpaceInfo>(environment.InstanceID);
            var observationSpaceInfo = await this.client.EnvObservationSpaceInfo<CartPoleObservationSpaceInfo>(environment.InstanceID);

            // Rebid our velocity and angular velocity parameters, the pole should stand still as much as possible
            observationSpaceInfo.Info.High[1] = 0.5;
            observationSpaceInfo.Info.Low[1] = -0.5;
            observationSpaceInfo.Info.High[3] = ConvertToRadians(50);
            observationSpaceInfo.Info.Low[3] = -ConvertToRadians(50);
            
            // Set up the new learning agent
            this.agent = new CartPoleAgent(this.client, this.environment, actionSpaceInfo.Info.N, observationSpaceInfo.Info);
        }

        /**
         * An experiment will run, whereafter it will constantly repeat the following loop:
         * 1. Observe environment state
         * 2. Based on the observation take an action
         * 3. Calculate the reward for the action and update our policy
         *
         * This we will do EPISODE_COUNT times or until we converge (when we have a convergence detector)
         */
        public async Task Run()
        {
            await this.Initialize();

            Stopwatch sw = new Stopwatch();

            // @todo: start monitoring
            for (var episode = 0; episode < EPISODE_COUNT; episode++) {
                sw.Start();

                // Reset the whole environment to start over
                var environment = new EnvStepResponse<double[]>(){
                    IsDone = false,
                    Observation = (await this.client.EnvReset<double[]>(this.environment.InstanceID)).Observation,
                    Reward = 0.0
                };

                // Keep executing while we can:
                // if environment.IsDone, then the pole tipped to far, or we died so we stop
                // if t >= maxTimeSteps then we did not solve it fast enough
                var t = 0;
                while (!environment.IsDone && t < MAX_TIME_STEPS) {
                    environment = await this.agent.Act(episode, environment.Observation);
                    t++;
                }

                sw.Stop();
                Console.WriteLine($"Episode {episode} ended after {t} timesteps in {sw.Elapsed}");
                sw.Reset();
            }

            // @todo: stop monitoring
        }

        private double ConvertToRadians(double angle) 
        {
            return (Math.PI / 180) * angle;
        }
    }
}