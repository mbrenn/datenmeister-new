using System;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.TypeSupport
{
    /// <summary>
    /// The default type integrator performs the following actions:
    /// a) Copies the primitive types from the uml namespace
    /// b) Creates a DateTime type instance
    /// c) Add additional default types likes packages from the Default.xmi
    /// </summary>
    public class DefaultTypeIntegrator
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IntegrationSettings _integrationSettings;
        private readonly PackageMethods _packageMethods;

        public DefaultTypeIntegrator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _integrationSettings = scopeStorage.Get<IntegrationSettings>();
            _packageMethods = new PackageMethods(_workspaceLogic);
        }

        /// <summary>
        /// Creates the default types in the types workspaces
        /// </summary>
        public void CreateDefaultTypesForTypesWorkspace()
        {
            var typeWorkspace = _workspaceLogic.GetTypesWorkspace();

            // Copies the Primitive Types to the internal types, so it is available for everybody, we will create a new extent for this
            var primitiveTypes = new MofUriExtent(
                new InMemoryProvider(),
                WorkspaceNames.UriExtentPrimitiveTypes);
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
                        .FindElementByUri(WorkspaceNames.UriExtentPrimitiveTypes + "#_0")
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
                if (package == null)
                    throw new InvalidOperationException("PrimitiveTypes could not be created");
                
                var umlData = _workspaceLogic.GetUmlData();

                var dateTime = factory.create(umlData.SimpleClassifiers.__PrimitiveType);
                ((ICanSetId) dateTime).Id = "PrimitiveTypes.DateTime";
                dateTime.set(_UML._CommonStructure._NamedElement.name, "DateTime");
                PackageMethods.AddObjectToPackage(package, dateTime);
            }
        }
    }
}