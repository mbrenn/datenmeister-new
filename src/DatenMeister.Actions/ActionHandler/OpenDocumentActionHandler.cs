using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler
{
    public class OpenDocumentActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__DocumentOpenAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var filePath = action.getOrDefault<string>(_DatenMeister._Actions._DocumentOpenAction.filePath);
            
            filePath = Environment.ExpandEnvironmentVariables(filePath);

            DotNetHelper.CreateProcess(filePath);
        }
    }
}