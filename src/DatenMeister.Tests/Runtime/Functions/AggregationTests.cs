using DatenMeister.EMOF.InMemory;
using DatenMeister.Runtime.Functions.Aggregation;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Functions
{
    [TestFixture]
    public class AggregationTests
    {
        [Test]
        public  void TestMax()
        {
            object property1;
            var reflectiveSequence = CreateReflectiveSequence(out property1);

            var aggregator = new MaxAggregator(property1);
            var finalValue  = aggregator.Aggregate(reflectiveSequence);

            Assert.That(finalValue, Is.EqualTo(4));
        }

        [Test]
        public void TestMin()
        {
            object property1;
            var reflectiveSequence = CreateReflectiveSequence(out property1);

            var aggregator = new MinAggregator(property1);
            var finalValue = aggregator.Aggregate(reflectiveSequence);

            Assert.That(finalValue, Is.EqualTo(1));
        }

        [Test]
        public void TestSum()
        {
            object property1;
            var reflectiveSequence = CreateReflectiveSequence(out property1);

            var aggregator = new SumAggregator(property1);
            var finalValue = aggregator.Aggregate(reflectiveSequence);

            Assert.That(finalValue, Is.EqualTo(13));
        }

        [Test]
        public void TestAverage()
        {
            object property1;
            var reflectiveSequence = CreateReflectiveSequence(out property1);

            var aggregator = new AverageAggregator(property1);
            var finalValue = aggregator.Aggregate(reflectiveSequence);

            Assert.That(finalValue, Is.EqualTo(2.6));
        }

        [Test]
        public void TestCount()
        {
            object property1;
            var reflectiveSequence = CreateReflectiveSequence(out property1);

            var aggregator = new CountAggregator(property1);
            var finalValue = aggregator.Aggregate(reflectiveSequence);

            Assert.That(finalValue, Is.EqualTo(5));
        }

        private static MofReflectiveSequence CreateReflectiveSequence(out object property1)
        {
            property1 = new object();
            var property2 = new object();
            var reflectiveSequence = new MofReflectiveSequence();

            var value = new MofObject();
            value.set(property1, 3);
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 1);
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 2);
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 3);
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 4);
            value.set(property2, 4);
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property2, 3);
            reflectiveSequence.add(value);
            return reflectiveSequence;
        }
    }
}
