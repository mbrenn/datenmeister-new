using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Benchmark.Integration
{
    public class UmlBenchmarks
    {
        private IExtent? _umlElements;

        public UmlBenchmarks()
        {
            Task.Run(async () =>
            {
                await using var datenMeister = await DatenMeisterBenchmark.GetDatenMeisterScope();
                _umlElements = datenMeister.WorkspaceLogic.GetUmlWorkspace().extent
                                   .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == "dm:///_internal/model/uml")
                               ?? throw new InvalidOperationException("Uml extent is not found");

                if (_umlElements == null)
                {
                    throw new InvalidOperationException("UmlElements == null");
                }
            }).GetAwaiter().GetResult();
        }

        [Benchmark]
        public void GetAllUmlDescendents()
        {
            var totalName = _umlElements!.elements().GetAllDescendantsIncludingThemselves().Count();
            Console.WriteLine(totalName);
        }

        [Benchmark]
        public void GetFirst3SpecializedClasses()
        {
            var totalName = _umlElements!.elements().GetAllDescendantsIncludingThemselves()
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
            var totalName = _umlElements!.elements().GetAllDescendantsIncludingThemselves()
                .OfType<IElement>()
                .Where(x => x.metaclass?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
                .SelectMany(x => ClassifierMethods.GetSpecializations(x))
                .ToList()
                .Count;

            Console.WriteLine(totalName);
        }
    }
}