using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Types;

/// <summary>
/// The default type integrator performs the following actions:
/// a) Copies the primitive types from the uml namespace
/// b) Creates a DateTime type instance
/// c) Add additional default types likes packages from the Default.xmi
/// </summary>
public class DefaultTypeIntegrator
{
    private readonly IntegrationSettings _integrationSettings;
    private readonly IScopeStorage _scopeStorage;
    private readonly IWorkspaceLogic _workspaceLogic;

    public DefaultTypeIntegrator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
        _integrationSettings = scopeStorage.Get<IntegrationSettings>();
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
            WorkspaceNames.UriExtentPrimitiveTypes,
            _scopeStorage);
        primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespace);
        primitiveTypes.AddAlternativeUri(WorkspaceNames.StandardPrimitiveTypeNamespaceAlternative);

        if (!_integrationSettings.PerformSlimIntegration)
        {
            // Looks for the primitive type in the UML Workspace
            var foundPackage =
                PackageMethods.GetOrCreatePackageStructure(primitiveTypes.elements(), "PrimitiveTypes");

            _workspaceLogic.AddExtent(typeWorkspace, primitiveTypes);

            // Copy the primitive type into a new extent for the type workspace
            CopyMethods.CopyToElementsProperty(
                _workspaceLogic.GetUmlWorkspace()
                    .FindObject(WorkspaceNames.UriExtentPrimitiveTypes + "#_0")
                    ?.get(_UML._Packages._Package.packagedElement) as IReflectiveCollection
                ?? throw new InvalidOperationException("PrimitiveTypes is not found"),
                foundPackage,
                _UML._Packages._Package.packagedElement,
                CopyOptions.CopyId);

            // Create the Primitive Type for the .Net-Type: DateTime
            var internalUserExtent = LocalTypeSupport.GetInternalTypeExtent(_workspaceLogic);
            var factory = new MofFactory(internalUserExtent);
            var package =
                PackageMethods.GetOrCreatePackageStructure(internalUserExtent.elements(), "PrimitiveTypes");
            if (package == null)
                throw new InvalidOperationException("PrimitiveTypes could not be created");

            var dateTime = factory.create(_UML.TheOne.SimpleClassifiers.__PrimitiveType);
            ((ICanSetId) dateTime).Id = "PrimitiveTypes.DateTime";
            dateTime.set(_UML._CommonStructure._NamedElement.name, "DateTime");
            PackageMethods.AddObjectToPackage(package, dateTime);
        }
    }
}