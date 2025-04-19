using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Benchmark.Providers
{
    public class InMemoryProviderBenchmark
    {
        [Benchmark]
        public void AddItems()
        {
            var memoryProvider = new InMemoryProvider();
            var benchmarkExtent = new MofUriExtent(memoryProvider, "dm:///benchmark", null);
            
            var mofFactory = new MofFactory(benchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                benchmarkExtent.elements().add(mofFactory.create(null));
            }
        }

        [Benchmark]
        public void DeleteItemsForward()
        {            
            var filledMemoryProvider = new InMemoryProvider();
            var filledBenchmarkExtent = new MofUriExtent(filledMemoryProvider, "dm:///benchmark", null);

            // Fill the queue
            var mofFactory = new MofFactory(filledBenchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                filledBenchmarkExtent.elements().add(mofFactory.create(null));
            }
            
            // Delete the queue
            var items = filledBenchmarkExtent.elements().ToList();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items were found");
            }

            foreach (var item in items)
            {
                filledBenchmarkExtent.elements().remove(item);
            }
        }

        [Benchmark]
        public void DeleteItemsReverse()
        {
            var filledMemoryProvider = new InMemoryProviderLinkedList();
            var filledBenchmarkExtent = new MofUriExtent(filledMemoryProvider, "dm:///benchmark", null);

            // Fill the queue
            var mofFactory = new MofFactory(filledBenchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                filledBenchmarkExtent.elements().add(mofFactory.create(null));
            }
            
            // Delete the queue
            var items = filledBenchmarkExtent.elements().ToList();
            items.Reverse();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items were found");
            }

            foreach (var item in items)
            {
                filledBenchmarkExtent.elements().remove(item);
            }
        }
        
        [Benchmark]
        public void AddItemsLinkedList()
        {
            var memoryProvider = new InMemoryProviderLinkedList();
            var benchmarkExtent = new MofUriExtent(memoryProvider, "dm:///benchmark", null);
            
            var mofFactory = new MofFactory(benchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                benchmarkExtent.elements().add(mofFactory.create(null));
            }
        }

        [Benchmark]
        public void DeleteItemsForwardLinkedList()
        {            
            var filledMemoryProvider = new InMemoryProviderLinkedList();
            var filledBenchmarkExtent = new MofUriExtent(filledMemoryProvider, "dm:///benchmark", null);

            // Fill the queue
            var mofFactory = new MofFactory(filledBenchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                filledBenchmarkExtent.elements().add(mofFactory.create(null));
            }
            
            // Delete the queue
            var items = filledBenchmarkExtent.elements().ToList();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items were found");
            }

            foreach (var item in items)
            {
                filledBenchmarkExtent.elements().remove(item);
            }
        }

        [Benchmark]
        public void DeleteItemsReverseLinkedList()
        {
            var filledMemoryProvider = new InMemoryProviderLinkedList();
            var filledBenchmarkExtent = new MofUriExtent(filledMemoryProvider, "dm:///benchmark", null);

            // Fill the queue
            var mofFactory = new MofFactory(filledBenchmarkExtent);
            for (var n = 0; n < 10000; n++)
            {
                filledBenchmarkExtent.elements().add(mofFactory.create(null));
            }
            
            // Delete the queue
            var items = filledBenchmarkExtent.elements().ToList();
            items.Reverse();
            if (items.Count == 0)
            {
                throw new InvalidOperationException("No items were found");
            }

            foreach (var item in items)
            {
                filledBenchmarkExtent.elements().remove(item);
            }
        }
    }
}