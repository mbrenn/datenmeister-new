using System;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions
{
    /// <summary>
    /// This is just an echo action handler which returns a success in case the
    /// parameter 'shallSuccess' is 'OK'.
    /// This handler is just for web integration testing
    /// </summary>
    public class EchoActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__EchoAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            if (action.getOrDefault<string>(_DatenMeister._Actions._EchoAction.shallSuccess) == "OK")
            {
                return;
            }

            throw new InvalidOperationException("shall Success is not OK");
        }
    }
}