using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
// Created by $DatenMeister.SourcecodeGenerator.DotNetIntegrationGenerator
// ReSharper disable RedundantNameQualifier

namespace DatenMeister.Models.ManagementProviders
{
    public static class IntegrateManagementProvider
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
                var type = typeof(DatenMeister.Runtime.ExtentStorage.ExtentLoadingState);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.ManagementProvider.Extent);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.ManagementProvider.Workspace);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Runtime.ExtentTypeSetting);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Runtime.ExtentProperties);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Runtime.ExtentPropertyDefinition);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
            {
                var type = typeof(DatenMeister.Models.Runtime.ExtentSettings);
                var typeAsElement = generator.CreateTypeFor(type);
                collection.add(typeAsElement);
                extent.TypeLookup.Add(typeAsElement, type);
            }
        }
    }
}
