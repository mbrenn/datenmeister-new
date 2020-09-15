using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;

namespace DatenMeister.Modules.Actions.ActionHandler
{
    public class DropWorkspaceActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _Actions.TheOne.__DropWorkspaceAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            throw new InvalidOperationException();
        }
    }
}