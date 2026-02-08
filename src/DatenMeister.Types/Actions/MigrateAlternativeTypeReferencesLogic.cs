using System.Xml.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Types.Actions;

/// <summary>
/// Walks through all given workspaces and changes a potential metaclass which is referencing to
/// an alternative metaclass definition.
/// This is useful in case an extent-uri is maintained, but we keep the old reference as
/// an alternative metaclass definition to support backward compatibility.
/// </summary>
public class MigrateAlternativeTypeReferencesActionLogic(IWorkspaceLogic workspaceLogic)
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger logger = new ClassLogger(typeof(MigrateAlternativeTypeReferencesActionLogic));

    /// <summary>
    /// Defines the workspaces to be considered
    /// </summary>
    public List<string> WorkspacesToBeConsidered { get; set; } =
        [WorkspaceNames.WorkspaceData, WorkspaceNames.WorkspaceTypes];

    /// <summary>
    /// Performs the migration
    /// </summary>
    public Task MigrateAsync()
    {
        return Task.Run(() =>
        {
            using var stopWatchLogger = new StopWatchLogger(logger, "Migrating alternative type references");

            // 1) Collect all mapping from alternative URIs to primary URIs
            var mapping = new Dictionary<string, string>();
            foreach (var workspace in workspaceLogic.Workspaces)
            {
                foreach (var extent in workspace.extent)
                {
                    if (extent is not (IUriExtent uriExtent and IHasAlternativeUris hasAlternativeUris))
                        continue;

                    var primaryUri = uriExtent.contextURI();
                    foreach (var alternativeUri in hasAlternativeUris.AlternativeUris)
                    {
                        if (!string.IsNullOrEmpty(alternativeUri))
                        {
                            mapping.TryAdd(alternativeUri, primaryUri);
                        }
                    }
                }
            }

            if (mapping.Count == 0)
            {
                logger.Info("No alternative URIs found for migration.");
                return;
            }

            foreach (var workspaceId in WorkspacesToBeConsidered)
            {
                var workspace = workspaceLogic.GetWorkspace(workspaceId);
                if (workspace == null)
                {
                    logger.Warn($"Workspace {workspaceId} does not exist");
                    continue;
                }

                // 1) Get all the extents of the workspace and try to get the Provider
                foreach (var extent in workspace.extent)
                {
                    // 2) Convert the Provider to XmiProvider, in case the provider is not such a provider, skip it
                    if (extent is MofUriExtent { Provider: XmiProvider xmiProvider } mofUriExtent)
                    {
                        // 4) In total, add also an information message via class logger that the extent is currently evaluated
                        logger.Info($"Evaluating extent: {mofUriExtent.contextURI()}");

                        // 3) Walk through all elements of the Xml and check that the type is fitting to one of the alternative
                        // Extent-Uris. If that it the case, rename it to the real uri of the extent. Potential Fragments or
                        // Queries must be maintained. Just work via Xml, do not try to use getMetaClass or any other IObject
                        // or IElement method
                        MigrateXml(xmiProvider.Document, mapping);
                    }
                }
            }
        });
    }

    /// <summary>
    /// Migrates the XML document by replacing alternative URIs with primary URIs
    /// </summary>
    /// <param name="document">The XML document to migrate</param>
    /// <param name="mapping">The mapping of alternative URIs to primary URIs</param>
    private static void MigrateXml(XDocument document, Dictionary<string, string> mapping)
    {
        var typeAttributeName = Namespaces.Xmi + "type";

        foreach (var element in document.Descendants())
        {
            var typeAttribute = element.Attribute(typeAttributeName);
            if (typeAttribute == null) continue;

            var typeValue = typeAttribute.Value;
            var hasBeenMigrated = false;

            // First check the mapping from alternative URIs
            foreach (var (alternativeUri, primaryUri) in mapping)
            {
                if (typeValue.StartsWith(alternativeUri + "#"))
                {
                    var newValue = primaryUri + typeValue.Substring(alternativeUri.Length);
                    typeAttribute.Value = newValue;
                    logger.Debug($"Migrated type (mapping): {typeValue} -> {newValue}");
                    hasBeenMigrated = true;
                    break;
                }
            }

            if (!hasBeenMigrated)
            {
                // If not migrated by mapping, use the global migration helper
                var migratedUri = MofUriExtent.Migration.MigrateUriForResolver(typeValue);
                if (migratedUri != null && migratedUri != typeValue)
                {
                    typeAttribute.Value = migratedUri;
                    logger.Debug($"Migrated type (resolver): {typeValue} -> {migratedUri}");
                }
            }
        }
    }
}