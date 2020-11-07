using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Functions.Aggregation;
using DatenMeister.Runtime.Functions.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using NUnit.Framework;
using System.Collections.Generic;

namespace DatenMeister.Tests.Runtime.Functions
{
    [TestFixture]
    public class AggregationTests
    {
        private string property1 = "Prop1";
        private string property2 = "Prop2";
        private string property3 = "Prop3";

        [Test]
        public void TestMax()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new MaxAggregator();
            var finalValue = reflectiveSequence.Aggregate<double>(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(4));
        }

        [Test]
        public void TestMin()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new MinAggregator();
            var finalValue = reflectiveSequence.Aggregate<double>(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(1));
        }

        [Test]
        public void TestSum()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new SumAggregator();
            var finalValue = reflectiveSequence.Aggregate<double>(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(13));
        }

        [Test]
        public void TestAverage()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new AverageAggregator();
            var finalValue = reflectiveSequence.Aggregate<double>(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(2.6));
        }

        [Test]
        public void TestCount()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            var aggregator = new CountAggregator();
            var finalValue = reflectiveSequence.Aggregate<double>(aggregator, property1);

            Assert.That(finalValue, Is.EqualTo(5));
        }

        [Test]
        public void TestGroupBy()
        {
            var reflectiveSequence = CreateReflectiveSequence();

            Func<IAggregator> aggregatorFunc = () => new SumAggregator();
            var finalValue =
                reflectiveSequence.GroupProperties(
                        property3,
                        property1,
                        aggregatorFunc,
                        property1)
                    .Cast<IObject>()
                    .ToList();
            
            var groupByA = finalValue.FirstOrDefault(x => x.get(property3).ToString() == "A");
            var groupByB = finalValue.FirstOrDefault(x => x.get(property3).ToString() == "B");

            Assert.That(groupByA, Is.Not.Null);
            Assert.That(groupByB, Is.Not.Null);

            Assert.That(groupByA.get(property1), Is.EqualTo(9));
            Assert.That(groupByB.get(property1), Is.EqualTo(4));

        }

        private IReflectiveCollection CreateReflectiveSequence()
        {
            var provider = new InMemoryProvider();
            var extent = new MofUriExtent(provider, "dm:///test");
            var element = new MofObject(new InMemoryObject(provider), extent);
            var factory = new MofFactory(extent);

            element.set("c", new List<object>());
            var reflectiveSequence = (IReflectiveCollection) element.get("c");

            var value = factory.create(null);
            value.set(property1, 3);
            value.set(property3, "A"); // A = 3
            reflectiveSequence.add(value);

            value = factory.create(null);
            value.set(property1, 1);
            value.set(property3, "B"); // A = 3, B = 1
            reflectiveSequence.add(value);

            value = factory.create(null);
            value.set(property1, 2);
            value.set(property3, "A"); // A = 5, B = 1
            reflectiveSequence.add(value);

            value = factory.create(null);
            value.set(property1, 3);
            value.set(property3, "B"); // A = 5, B = 4
            reflectiveSequence.add(value);

            value = factory.create(null);
            value.set(property1, 4);
            value.set(property2, 4);
            value.set(property3, "A"); // A = 9, B = 4
            reflectiveSequence.add(value);

            value = factory.create(null);
            value.set(property2, 3);
            reflectiveSequence.add(value);
            return reflectiveSequence;
        }
    }
}
