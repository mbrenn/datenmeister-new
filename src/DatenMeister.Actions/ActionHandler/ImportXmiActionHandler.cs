using System.Xml.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

/// <summary>
/// Performs an import of items into an extent or into a properties extent
/// </summary>
public class ImportXmiActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _DatenMeister.TheOne.Actions.__ImportXmiAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var workspace = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.workspaceId);
            var itemUri = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.itemUri);
            var xmi = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.xmi);

            var targetObject = actionLogic.WorkspaceLogic.FindObject(workspace, itemUri);

            var xmlDocument = XDocument.Parse(xmi);
            if (targetObject is IExtent targetExtent)
            {
                var provider = new XmiProvider(xmlDocument);
                var extent = new MofUriExtent(provider, "dm:///import", actionLogic.ScopeStorage);

                // Now do the copying. it makes us all happy
                var extentCopier = new ExtentCopier(new MofFactory(targetExtent));
                extentCopier.Copy(extent.elements(), targetExtent.elements());
            }
            else if (targetObject != null)
            {
                // Ok, we have an item, create a demo extent and add it to the existing extent
                var property = action.getOrDefault<string>(_DatenMeister._Actions._ImportXmiAction.property);
                var addToCollection =
                    action.getOrDefault<bool>(_DatenMeister._Actions._ImportXmiAction.addToCollection);

                var provider = new XmiProvider();
                var extent = new MofUriExtent(provider, "dm:///import", actionLogic.ScopeStorage);
                var item = new XmiProviderObject(
                    xmlDocument.Root ?? throw new InvalidOperationException("Root Element is not set"),
                    provider);
                var mofElement = new MofElement(item, extent);

                if (addToCollection)
                {
                    targetObject.AddCollectionItem(property, mofElement);
                }
                else
                {
                    targetObject.set(property, mofElement);
                }
            }
            else
            {
                throw new InvalidOperationException("The target object has not been found");
            }
        });

        return null;
    }
}