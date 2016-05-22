using System;
using System.Linq;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Aggregation;
using DatenMeister.Runtime.Functions.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Functions
{
    [TestFixture]
    public class AggregationTests
    {
        private object property1;
        private object property2;
        private object property3;

        [Test]
        public void TestMax()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new MaxAggregator();
            var finalValue = reflectiveSequence.Aggregate(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(4));
        }

        [Test]
        public void TestMin()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new MinAggregator();
            var finalValue = reflectiveSequence.Aggregate(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(1));
        }

        [Test]
        public void TestSum()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new SumAggregator();
            var finalValue = reflectiveSequence.Aggregate(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(13));
        }

        [Test]
        public void TestAverage()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new AverageAggregator();
            var finalValue = reflectiveSequence.Aggregate(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(2.6));
        }

        [Test]
        public void TestCount()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new CountAggregator();
            var finalValue = reflectiveSequence.Aggregate(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(5));
        }

        [Test]
        public void TestGroupBy()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            Func<IAggregator<double>> aggregatorFunc = () => new SumAggregator();
            var finalValue = reflectiveSequence.GroupBy(
                property3, 
                property1, 
                aggregatorFunc).Cast<IObject>();

            var groupByA = finalValue.FirstOrDefault(x => x.get(property3).ToString() == "A");
            var groupByB = finalValue.FirstOrDefault(x => x.get(property3).ToString() == "B");

            Assert.That(groupByA, Is.Not.Null);
            Assert.That(groupByB, Is.Not.Null);

            Assert.That(groupByA.get(property1), Is.EqualTo(9));
            Assert.That(groupByB.get(property1), Is.EqualTo(4));

        }

        private MofReflectiveSequence CreateReflectiveSequence()
        {
            property1 = new object();
            property2 = new object();
            property3 = new object();
            var reflectiveSequence = new MofReflectiveSequence();

            var value = new MofObject();
            value.set(property1, 3);
            value.set(property3, "A"); // A = 3
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 1);
            value.set(property3, "B"); // A = 3, B = 1
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 2);
            value.set(property3, "A"); // A = 5, B = 1
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 3);
            value.set(property3, "B"); // A = 5, B = 4
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property1, 4);
            value.set(property2, 4);
            value.set(property3, "A"); // A = 9, B = 4
            reflectiveSequence.add(value);

            value = new MofObject();
            value.set(property2, 3);
            reflectiveSequence.add(value);
            return reflectiveSequence;
        }
    }
}
