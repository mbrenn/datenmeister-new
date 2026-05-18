using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Forms.Actions;

public class GetAttributeOfItemActionHandler(IWorkspaceLogic workspaceLogic) : IActionHandler
{
    
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _Actions.TheOne.Forms.__GetAttributeOfItem) == true;
    }

    public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        try
        {
            var itemUrl = action.getOrDefault<string>(_Actions._Forms._GetAttributeOfItem.itemUri);
            var workspace = action.getOrDefault<string>(_Actions._Forms._GetAttributeOfItem.workspace);
            var propertyName = action.getOrDefault<string>(_Actions._Forms._GetAttributeOfItem.propertyName);
        
            // Gets the item
            if (workspaceLogic.FindElement(workspace, itemUrl) is not MofElement foundItem)
            {
                return Task.FromResult<IElement?>(null);
            }
            
            // Gets the property
            var classModel = foundItem.GetClassModel();
            if (classModel == null)
            {
                return Task.FromResult<IElement?>(null);
            }

            var attribute = classModel.FindAttribute(propertyName);
            if (attribute == null)
            {
                return Task.FromResult<IElement?>(null);
            }

            var result = InMemoryObject.CreateEmpty(_Actions.TheOne.Forms.__GetAttributeOfItemResult);
            result.set(_Actions._Forms._GetAttributeOfItemResult.isComposite, attribute.IsComposite);
            result.set(_Actions._Forms._GetAttributeOfItemResult.isMultiple, attribute.IsMultiple);
            result.set(_Actions._Forms._GetAttributeOfItemResult.metaClassUri, attribute.TypeUrl);
            return Task.FromResult<IElement?>(result);
        }
        
        catch (Exception exception)
        {
            return Task.FromException<IElement?>(exception);
        }
    }
}