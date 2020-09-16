using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class LoadExtentActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__LoadExtentAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var configuration =
                action.getOrDefault<IElement>(_DatenMeister._Actions._LoadExtentAction.configuration);

            if (configuration == null)
            {
                throw new InvalidOperationException("No configuration is set");
            }
            
            throw new NotImplementedException("Not possible to load extent manager up to now");
        }
    }
}