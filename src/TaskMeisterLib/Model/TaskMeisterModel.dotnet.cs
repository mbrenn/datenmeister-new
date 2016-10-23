using DatenMeister;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;

namespace TaskMeister.Model
{
    public static class IntegrateTaskMeisterModel
    {
        /// <summary>
        /// Assigns the types of form and fields by converting the 
        /// .Net objects to DatenMeister elements and adds them into 
        /// the filler, the collection and also into the lookup. 
        /// </summary>
        /// <param name="uml">The uml metamodel to be used</param>
        /// <param name="factory">Factory being used for creation</param>
        /// <param name="collection">Collection that shall be filled</param>
        /// <param name="filledStructure">The form and fields structure</param>
        /// <param name="lookup">And finally the Dotnet type</param>
        public static void Assign(_UML uml,IFactory factory, IReflectiveCollection collection, _TaskMeisterModel filledStructure, IDotNetTypeLookup lookup)
        {
            var generator = new DotNetTypeGenerator(factory, uml);
            {
                var type = typeof(IActivity);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__IActivity = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(IPerson);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__IPerson = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
        }
    }
}
