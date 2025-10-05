using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Forms.Actions;

public class CreateFormUponViewActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Forms.__CreateFormUponViewAction) == true;
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        var targetWorkspace =
            action.getOrDefault<string>(_Actions._Forms._CreateFormUponViewAction.targetPackageWorkspace);
        var targetUri =
            action.getOrDefault<string>(_Actions._Forms._CreateFormUponViewAction.targetPackageUri);

        var owner = actionLogic.WorkspaceLogic.FindObjectOrCollection(targetWorkspace, targetUri);
        if (owner == null)
        {
            throw new InvalidOperationException($"Element not found: {targetWorkspace}::{targetUri}");
        }
        
        IReflectiveCollection? targetCollection = owner as IReflectiveCollection;
        if (targetCollection == null && owner is IExtent extent)
        {
            targetCollection = extent.elements(); 
        }
        if (targetCollection == null && owner is IObject element)
        {
            targetCollection = element.getOrDefault<IReflectiveCollection>(
                DefaultClassifierHints.GetDefaultPackagePropertyName(element));
        }

        if (targetCollection == null)
        {
            throw new InvalidOperationException("We could not retrieve the collection.");
        }
        
        // Ok, we made it... Now we add the item to the target
        var queryStatement = action.getOrDefault<IElement>(_Actions._Forms._CreateFormUponViewAction.query);
        var factory = new MofFactory(targetCollection);
        var targetObject = factory.create(_DataViews.TheOne.__QueryStatement);
        targetCollection.add(targetObject);

        ObjectCopier.CopyPropertiesStatic(queryStatement, targetObject);

        var url = targetObject.GetUri();

        var result = InMemoryObject.CreateEmpty(
            _Actions.TheOne.ParameterTypes.__CreateFormUponViewResult);
        result.set(_Actions._ParameterTypes._CreateFormUponViewResult.resultingPackageUrl, url);

        return Task.FromResult(result)!;
    }
}