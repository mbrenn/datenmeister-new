using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.DefaultTypes;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.TypeSupport
{
    public class DefaultTypeIntegrator
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly IntegrationSettings _integrationSettings;
        private readonly PackageMethods _packageMethods;

        public DefaultTypeIntegrator(IWorkspaceLogic workspaceLogic, LocalTypeSupport localTypeSupport, IntegrationSettings integrationSettings)
        {
            _workspaceLogic = workspaceLogic;
            _localTypeSupport = localTypeSupport;
            _integrationSettings = integrationSettings;
            _packageMethods = new PackageMethods(_workspaceLogic);
        }

        /// <summary>
        /// Creates the default types
        /// </summary>
        public void CreateDefaultTypes()
        {
            var typeWorkspace = _workspaceLogic.GetTypesWorkspace();

            // Copies the Primitive Types to the internal types, so it is available for everybody, we will create a new extent for this
            var primitiveTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriPrimitiveTypesExtent);
            primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespace);
            primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespaceAlternative);

            if (!_integrationSettings.PerformSlimIntegration)
            {
                // Looks for the primitive type in the UML Workspace
                var foundPackage =
                    _packageMethods.GetOrCreatePackageStructure(primitiveTypes.elements(), "PrimitiveTypes");
                _workspaceLogic.AddExtent(typeWorkspace, primitiveTypes);

                // Copy the primitive type into a new extent for the type workspace
                CopyMethods.CopyToElementsProperty(
                    (_workspaceLogic.GetUmlWorkspace()
                        .FindElementByUri("datenmeister:///_internal/xmi/primitivetypes?PrimitiveTypes")
                        ?.get(_UML._Packages._Package.packagedElement) as IReflectiveCollection)
                    ?? throw new InvalidOperationException("PrimitiveTypes is not found"),
                    foundPackage,
                    _UML._Packages._Package.packagedElement,
                    CopyOptions.CopyId);

                // Create the Primitive Type for the .Net-Type: DateTime
                var internalUserExtent = LocalTypeSupport.GetInternalTypeExtent(_workspaceLogic);
                var factory = new MofFactory(internalUserExtent);
                var package =
                    _packageMethods.GetOrCreatePackageStructure(internalUserExtent.elements(), "PrimitiveTypes");
                var umlData = _workspaceLogic.GetUmlData();

                var dateTime = factory.create(umlData.SimpleClassifiers.__PrimitiveType);
                ((ICanSetId) dateTime).Id = "PrimitiveTypes.DateTime";
                dateTime.set(_UML._CommonStructure._NamedElement.name, "DateTime");
                PackageMethods.AddObjectToPackage(package, dateTime);

                // Create the class for the default types
                _localTypeSupport.AddInternalType("Default", typeof(Package));
            }
        }
    }
}