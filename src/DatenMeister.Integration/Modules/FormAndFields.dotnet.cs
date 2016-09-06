using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;

namespace DatenMeister.Models.Forms
{
    public static class IntegrateFormAndFields
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
        public static void Assign(_UML uml,IFactory factory, IReflectiveCollection collection, _FormAndFields filledStructure, IDotNetTypeLookup lookup)
        {
            var generator = new DotNetTypeGenerator(factory, uml);
            {
                var type = typeof(DatenMeister.Models.Forms.Form);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__Form = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.FieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__FieldData = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.TextFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__TextFieldData = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DateTimeFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DateTimeFieldData = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DropDownFieldData);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DropDownFieldData = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Forms.DefaultViewForMetaclass);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__DefaultViewForMetaclass = typeAsElement;
                lookup.Add(typeAsElement, type);
            }
        }
    }
}
