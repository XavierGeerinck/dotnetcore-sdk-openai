using System;
using System.Collections.Generic;
using OpenAI.CartPole;
using System.Linq;
using Xunit;

namespace OpenAI.CartPole.Tests
{
    public class UtilsTest
    {

        public UtilsTest()
        {
        }

        [Fact]
        public void Test_Should_CorrectlyUseCartesianProduct()
        {
            var vectors = new List<List<int>>();
            vectors.Add(new List<int>() { 1, 2, 3 });
            vectors.Add(new List<int>() { 4, 5, 6 });

            var result = Utils.CartesianProduct(vectors).ToList();

            Assert.Equal(9, result.Count);
            Assert.Equal(1, result.ToArray()[0].ToArray()[0]);
            Assert.Equal(4, result.ToArray()[0].ToArray()[1]);
            Assert.Equal(3, result.ToArray()[8].ToArray()[0]);
            Assert.Equal(6, result.ToArray()[8].ToArray()[1]);
        }
    }
}
