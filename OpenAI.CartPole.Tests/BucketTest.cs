using System;
using System.Collections.Generic;
using OpenAI.CartPole;
using Xunit;

namespace OpenAI.CartPole.Tests
{
    public class BucketTest
    {

        public BucketTest()
        {
        }

        [Fact]
        public void Test_Should_AddValueBelowItsRangeInFirstBucket()
        {
            var bucket = new Bucket(4, -2.635, 3.698);
            Assert.Equal(0, bucket.GetBucketIdxForValue(-100));
        }

        [Fact]
        public void Test_Should_AddValueAboveItsRangeInLastBucket()
        {
            var bucketCount = 4;
            var bucket = new Bucket(bucketCount, -2.635, 3.698);
            Assert.Equal(bucketCount - 1, bucket.GetBucketIdxForValue(100));
        }


        [Fact]
        public void Test_Should_AddValueInCorrectBuckets()
        {
            var bucketCount = 4;
            var bucket = new Bucket(bucketCount, -2.635, 3.698);

            double[] numbersToTest = new double[] { -100, -2.635, -1.052, -1.051, 0, 0.54, 2.2, 3.698, 100 };

            foreach (var i in numbersToTest) {
                bucket.AddValueToBucket(i);
            }

            Assert.Equal(new List<double> { -100, -2.635, -1.052 }, bucket.Buckets[0]);
            Assert.Equal(new List<double> { -1.051, 0 }, bucket.Buckets[1]);
            Assert.Equal(new List<double> { 0.54 }, bucket.Buckets[2]);
            Assert.Equal(new List<double> { 2.2, 3.698, 100 }, bucket.Buckets[3]);
        }
    }
}
