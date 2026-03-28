using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Actions.ActionHandler;

public class StoreElementActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.__StoreElementAction) == true;
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var targetWorkspace =
            action.getOrDefault<string>(_Actions._StoreElementAction.workspace);
        var targetUri =
            action.getOrDefault<string>(_Actions._StoreElementAction.url);

        var targetElement = actionLogic.WorkspaceLogic.FindObjectOrCollection(targetWorkspace, targetUri);
        if (targetElement == null)
        {
            throw new InvalidOperationException($"Element not found: {targetWorkspace}::{targetUri}");
        }
        
        IReflectiveCollection? targetCollection = targetElement as IReflectiveCollection;
        if (targetCollection == null && targetElement is IExtent extent)
        {
            targetCollection = extent.elements(); 
        }
        if (targetCollection == null && targetElement is IObject element)
        {
            targetCollection = element.getOrDefault<IReflectiveCollection>(
                DefaultClassifierHints.GetDefaultPackagePropertyName(element));
        }

        if (targetCollection == null)
        {
            throw new InvalidOperationException("We could not retrieve the collection.");
        }

        var sourceElement = action.getOrDefault<IElement?>(_Actions._StoreElementAction.element);
        string uri;
        if (sourceElement != null)
        {
            var factory = new MofFactory(targetCollection);
            var targetObject = factory.create(sourceElement.metaclass);
            targetCollection.add(targetObject);
            ObjectCopier.CopyPropertiesStatic(sourceElement, targetObject);
            uri = targetObject.GetUri() ?? throw new InvalidOperationException("No uri found");
        }
        else
        {
            throw new InvalidOperationException("No source element is set");
        }

        var result = InMemoryObject.CreateEmpty(_Actions.TheOne.__TargetReferenceResult);
        result.set(_Actions._TargetReferenceResult.targetUrl, uri);
        result.set(_Actions._TargetReferenceResult.targetWorkspace, targetWorkspace);

        return Task.FromResult<IElement?>(result);
    }
}