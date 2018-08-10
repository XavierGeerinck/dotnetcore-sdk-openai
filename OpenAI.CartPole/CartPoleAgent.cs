using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenAI.CartPole.Models;
using OpenAI.SDK;
using OpenAI.SDK.Models.Response;
using OpenAI.CartPole.Interfaces;
using System.Linq;
using System.Diagnostics;

namespace OpenAI.CartPole
{
    public class CartPoleAgent
    {
        private const double GAMMA = 1.0; // Discount Factor
        private const double MIN_EPSILON = 0.1; // Exploration Factor
        private const double MIN_ALPHA = 0.1; // Learning Rate
        private const double ADA_DIVISOR = 25; 

        private ApiService client;
        private EnvCreateResponse environment;
        private int numberOfActions;
        private int numberOfObservations;
        private CartPoleObservationSpaceInfo observationSpaceInfo;
        private List<Bucket> buckets;
        private List<List<double>> states;
        private double[][] q;

        public CartPoleAgent(ApiService client, EnvCreateResponse environment, int numberOfActions, CartPoleObservationSpaceInfo observationSpaceInfo)
        {
            this.client = client;
            this.environment = environment;
            this.numberOfActions = numberOfActions;
            this.numberOfObservations = observationSpaceInfo.Shape[0];
            this.observationSpaceInfo = observationSpaceInfo;

            // Since CartPole is a continuous space and infinite, we discretize it with buckets
            this.buckets = this.InitializeBuckets(new int[] { 1, 1, 8, 10 });
            this.states = this.InitializeStates(this.buckets);
            this.q = this.InitializeQTable(this.states.Count, this.numberOfActions);
        }

        private List<Bucket> InitializeBuckets(int[] bucketSizes)
        {
            var buckets = new List<Bucket>();

            for (var i = 0; i < this.numberOfObservations; i++) {
                var bucketSize = bucketSizes[i];
                var upperBound = this.observationSpaceInfo.High[i];
                var lowerBound = this.observationSpaceInfo.Low[i];
                buckets.Add(new Bucket(bucketSize, lowerBound, upperBound));
            }

            return buckets;
        }

        private List<List<double>> InitializeStates(List<Bucket> buckets)
        {
            var bucketIndexes = new List<List<double>>();

            buckets.ForEach(bucket => bucketIndexes.Add(bucket.BucketIndexes.Select(i => (double)i).ToList()));

            var states = Utils.CartesianProduct(bucketIndexes);

            Console.WriteLine(states);

            return states.Select(i => i.ToList()).ToList();
        }

        private double[][] InitializeQTable(int stateCount, int actionCount)
        {
            // Create a jagged array (=outer size is specified, inner size is dynamic)
            double[][] q = new double[stateCount][];

            for (var stateIdx = 0; stateIdx < stateCount; stateIdx++) {
                q[stateIdx] = new double[actionCount];

                for (var actionIdx = 0; actionIdx < actionCount; actionIdx++) {
                    q[stateIdx][actionIdx] = 0;
                }
            }

            return q;
        }
        
        /**
         * Convert an observation to a state index
         * - Our state is an array of numbers representing the different states we can be in, 
         *   think of it as a HashMap<int, List<int>> where the key is our state index and the value the observation bucket indexes
         * - Our observation is a array of numbers
         * Check where it belongs, depending on the buckets
         */
        private int ObservationToStateIndex(List<List<double>> states, List<Bucket> bucketArrays, double[] observations)
        {
            List<double> observationBucketIndexes = new List<double>();

            for (var i = 0; i < observations.Length; i++) {
                observationBucketIndexes.Add(bucketArrays[i].GetBucketIdxForValue(observations[i]));
            }

            // We will now find our state index. To do this we go over the state combinations and get the HashCode and compare it to our current HashCode
            // var stateIndex = states.FindIndex(i => i.GetHashCode() == observationBucketIndexes.GetHashCode());
            var stateIndex = states.Select((arr, idx) => new { idx, First = arr[0], Second = arr[1], Third = arr[2], Fourth = arr[3]  })
                                   .Where(t => t.First == observationBucketIndexes[0] && t.Second == observationBucketIndexes[1] && t.Third == observationBucketIndexes[2] && t.Fourth == observationBucketIndexes[3])
                                   .Select(t => t.idx)
                                   .ToList();

            // FindIndex returns -1 if not found
            // https://msdn.microsoft.com/en-us/library/x1xzf2ca%28v=vs.110%29.aspx?f=255&MSPPError=-2147217396 
            if (stateIndex[0] == -1) {
                throw new StateNotFoundException();
            }

            return stateIndex[0];
        }

        /**
         * Learning Factor
         * t is used to reduce learning over time
         */
        private double GetAlpha(int t)
        {
            return Math.Max(MIN_ALPHA, Math.Min(0.5, 1.0 - Math.Log10((t + 1) / ADA_DIVISOR)));
        }

        /**
         * Exploration Factor
         * t is used to reduce exploration over time
         */
        private double GetEpsilon(int t)
        {
            return Math.Max(MIN_EPSILON, Math.Min(1.0, 1.0 - Math.Log10((t + 1) / ADA_DIVISOR)));
        }

        private int ChooseAction(int stateIndex, double epsilon)
        {
            // Pick a random number, and if it's smaller then epsilon, then we pick a random action
            // This is what we call exploration
            Random rnd = new Random();
            var isRandom = (rnd.NextDouble() <= epsilon);

            if (isRandom) {
                return (int)Math.Floor(rnd.NextDouble() * this.numberOfActions);
            }
            
            var maxValue = this.q[stateIndex].Max();
            var maxIndex = this.q[stateIndex].ToList().IndexOf(maxValue);
            return maxIndex;
        }

        public void UpdateQ(int oldStateIndex, int action, double reward, int newStateIndex, double alpha)
        {
            this.q[oldStateIndex][action] += alpha * (reward + GAMMA * this.q[newStateIndex].Max() - this.q[oldStateIndex][action]);
        }

        public async Task<EnvStepResponse<double[]>> Act(int episode, double[] observation)
        {
            // ================================================================================================
            // 1. Map the current observation to a state
            // ================================================================================================
            // We get a number of values back, convert these to states
            var stateIndex = this.ObservationToStateIndex(this.states, this.buckets, observation);

            // Learning discount factor and learning rate
            var epsilon = this.GetEpsilon(episode);
            var alpha = this.GetAlpha(episode);

            // ================================================================================================
            // 2. Based on our observation, choose and take an action and view the change in environment
            // ================================================================================================
            // Choose an action
            var action = this.ChooseAction(stateIndex, epsilon);
            var newEnvironment = await this.client.EnvStep<double[]>(this.environment.InstanceID, action, true);

            // ================================================================================================
            // 3. Based on result of the action, update our Q value
            // ================================================================================================
            var newStateIndex = this.ObservationToStateIndex(this.states, this.buckets, newEnvironment.Observation);
            this.UpdateQ(stateIndex, action, newEnvironment.Reward, newStateIndex, alpha);

            return newEnvironment;
        }
    }
}