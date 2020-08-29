using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Benchmark.Integration
{
    public class UmlBenchmarks
    {
        private IExtent _umlElements;

        public UmlBenchmarks()
        {
            using var datenMeister = DatenMeisterBenchmark.GetDatenMeisterScope();
            _umlElements = datenMeister.WorkspaceLogic.GetUmlWorkspace().extent
                .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == "dm:///_internal/model/uml");

            if (_umlElements == null)
            {
                throw new InvalidOperationException("UmlElements == null");
            }
        }

        [Benchmark]
        public void GetAllUmlDescendents()
        {
            var totalName = _umlElements.elements().GetAllDescendantsIncludingThemselves().Count();
            Console.WriteLine(totalName);
        }

        [Benchmark]
        public void GetFirst3SpecializedClasses()
        {
            var totalName = _umlElements.elements().GetAllDescendantsIncludingThemselves()
                .OfType<IElement>()
                .Where(x => x.metaclass?.Equals( _UML.TheOne.StructuredClassifiers.__Class) == true)
                .Take(3)
                .SelectMany(x => ClassifierMethods.GetSpecializations(x))
                .ToList()
                .Count;

            Console.WriteLine(totalName);
        }

        [Benchmark]
        public void GetAllSpecializedClasses()
        {
            var totalName = _umlElements.elements().GetAllDescendantsIncludingThemselves()
                .OfType<IElement>()
                .Where(x => x.metaclass?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
                .SelectMany(x => ClassifierMethods.GetSpecializations(x))
                .ToList()
                .Count;

            Console.WriteLine(totalName);
        }
    }
}