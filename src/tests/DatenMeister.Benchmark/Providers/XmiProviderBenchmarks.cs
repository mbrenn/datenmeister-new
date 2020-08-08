using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BenchmarkDotNet.Attributes;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;

namespace DatenMeister.Benchmark.Providers
{
    public class XmiProviderBenchmarks
    {
        private readonly List<IElement> _elements;

        public XmiProviderBenchmarks()
        {
            var provider = new XmiProvider(
                XDocument.Parse("" +
                                "<item>" +
                                "    <item name=\"name1\" age=\"18\" />" +
                                "    <item name=\"name2\" age=\"25\" />" +
                                "    <item name=\"name3\" age=\"32\" />" +
                                "</item>"));    
            var extent = new MofUriExtent(provider, "dm:///test");
            
            _elements = extent.elements().OfType<IElement>().ToList();
        }
        
        [Benchmark]
        public void BenchMarkGetProperty()
        {
            foreach (var element in _elements)
            {
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
                element.getOrDefault<string>("name");
                element.getOrDefault<int>("age");
            }
        }
    }
}