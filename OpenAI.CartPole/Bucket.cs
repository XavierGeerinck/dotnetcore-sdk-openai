using System;
using System.Collections.Generic;

namespace OpenAI.CartPole 
{
    public class Bucket
    {
        private int bucketCount;
        private double rangeLow;
        private double rangeHigh;
        private double stepSize;
        public List<int> BucketIndexes { get; private set; }
        public List<List<double>> Buckets { get; private set; }
        
        public Bucket(int bucketCount, double rangeLow, double rangeHigh)
        {
            this.bucketCount = bucketCount;
            this.rangeLow = rangeLow;
            this.rangeHigh = rangeHigh;
            this.stepSize = (Math.Abs(this.rangeLow) + Math.Abs(this.rangeHigh)) / this.bucketCount;

            // Include the bucketIndexes for easiness of use
            this.BucketIndexes = new List<int>();
            for (var i = 0; i < this.bucketCount; i++) {
                this.BucketIndexes.Add(i);
            }

            // Init the buckets
            InitBuckets();
        }

        private void InitBuckets()
        {
            Buckets = new List<List<double>>();
            for (var i = 0; i < this.bucketCount; i++) {
                Buckets.Add(new List<double>());
            }
        }

        public void AddValueToBucket(double value)
        {
            var idx = GetBucketIdxForValue(value);
            Buckets[idx].Add(value);
        }

        public int GetBucketIdxForValue(double val)
        {
            var idx = 0;

            // Find bucket, put values on the outer size of the range in the last bucket
            while ((idx < this.bucketCount - 1) && val > this.rangeLow + this.stepSize * (idx + 1)) {
                idx++;
            }

            return idx;
        }
    }
}