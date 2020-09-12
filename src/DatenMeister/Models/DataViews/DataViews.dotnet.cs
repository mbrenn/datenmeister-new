using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Provider.DotNet;
// Created by $DatenMeister.SourcecodeGenerator.DotNetIntegrationGenerator
// ReSharper disable RedundantNameQualifier

namespace DatenMeister.Models.DataViews
{
    public static class IntegrateDataViews
    {
        /// <summary>
        /// Assigns the types of form and fields by converting the 
        /// .Net objects to DatenMeister elements and adds them into 
        /// the filler, the collection and also into the lookup. 
        /// </summary>
        /// <param name="uml">The uml metamodel to be used</param>
        /// <param name="factory">Factory being used for creation</param>
        /// <param name="collection">Collection that shall be filled</param>
        /// <param name="extent">And finally extent to which the types shall be registered</param>
        public static void Assign(IFactory factory, IReflectiveCollection collection, MofExtent extent)
        {
            var generator = new DotNetTypeGenerator(factory, extent);
            {
                var type = typeof(DatenMeister.Models.DataViews.DataView);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.ViewNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.SourceExtentNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.FlattenNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.FilterPropertyNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.FilterTypeNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.ComparisonMode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.SelectByFullNameNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.DataViews.DynamicSourceNode);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
        }
    }
}
