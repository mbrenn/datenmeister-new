using System;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler
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

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            await Task.Run(() =>
            {
                if (action.getOrDefault<string>(_DatenMeister._Actions._EchoAction.shallSuccess) == "OK")
                {
                    return;
                }

                throw new InvalidOperationException("The property 'shallSuccess' is not OK");
            });
            
            return null;
        }
    }
}