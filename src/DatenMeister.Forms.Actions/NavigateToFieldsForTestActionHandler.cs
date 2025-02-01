using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;

namespace DatenMeister.Forms.Actions
{
    internal class NavigateToFieldsForTestActionHandler : IActionHandler
    {
        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            // First, create a temporary object
            var temporaryObject = InMemoryObject.CreateEmpty();
            InMemoryProvider.TemporaryExtent.elements().add(temporaryObject);

            // Second, navigate the user to the recently created object with the testing form.


            return await Task.FromResult<IElement?>(null);
        }

        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Forms.__NavigateToFieldsForTestAction) == true;
        }
    }
}
