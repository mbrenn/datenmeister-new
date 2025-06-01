using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

/// <summary>
/// Implements the action handler to delete a certain property from all items within a given collection
/// </summary>
public class DeletePropertyFromCollectionActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__DeletePropertyFromCollectionAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var collectionUrl =
                action.getOrDefault<string>(
                    _Actions._DeletePropertyFromCollectionAction.collectionUrl);
            var propertyName =
                action.getOrDefault<string>(_Actions._DeletePropertyFromCollectionAction
                    .propertyName);

            if (string.IsNullOrEmpty(propertyName))
            {
                throw new InvalidOperationException("Property is not set");
            }

            if (string.IsNullOrEmpty(collectionUrl))
            {
                throw new InvalidOperationException("Collection Url is not set");
            }

            var resolvedElement = actionLogic.WorkspaceLogic.Resolve(
                collectionUrl, ResolveType.Default);

            var collection =
                resolvedElement as IReflectiveCollection
                ?? (resolvedElement is IUriExtent extent ? extent.elements() : null);

            if (collection == null)
            {
                throw new InvalidOperationException(
                    "The collection with uri '" + collectionUrl + "' was not found.");
            }

            // Remove the properties
            foreach (var element in collection.OfType<IObject>())
            {
                element.unset(propertyName);
            }
        });

        return null;
    }
}