using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Provider.DotNet;

// Created by $DatenMeister.SourcecodeGenerator.DotNetIntegrationGenerator

namespace DatenMeister.Excel.Models
{
    public static class IntegrateExcelModels
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
        /// <param name="extent">And finally extent to which the types shall be registered</param>
        public static void Assign(_UML uml, IFactory factory, IReflectiveCollection collection, _ExcelModels filledStructure, MofUriExtent extent)
        {
            var generator = new DotNetTypeGenerator(factory, uml, extent);
            {
                var type = typeof(DatenMeister.Excel.Models.Workbook);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__Workbook = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Excel.Models.Table);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                filledStructure.__Table = typeAsElement;
                extent.TypeLookup.Add(typeAsElement, type);
            }
        }
    }
}
