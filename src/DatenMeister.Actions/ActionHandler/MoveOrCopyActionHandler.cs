using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Modules.DefaultTypes;

namespace DatenMeister.Actions.ActionHandler
{
    public class MoveOrCopyActionHandler : IActionHandler
    {
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.equals(
                _DatenMeister.TheOne.Actions.__MoveOrCopyAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var source = action.getOrDefault<IObject>(_DatenMeister._Actions._MoveOrCopyAction.source)
                         ?? throw new InvalidOperationException("'Source' is not set.");
            var value = action.getOrDefault<IObject>(_DatenMeister._Actions._MoveOrCopyAction.target)
                        ?? throw new InvalidOperationException("'target' is not set");
            var actionType = action.getOrDefault<_DatenMeister._Actions.___MoveOrCopyType>(
                _DatenMeister._Actions._MoveOrCopyAction.actionType);

            if (actionType == _DatenMeister._Actions.___MoveOrCopyType.Copy)
            {
                ObjectOperations.CopyObject(
                    source,
                    value);
            }

            if (actionType == _DatenMeister._Actions.___MoveOrCopyType.Move)
            {
                ObjectOperations.MoveObject(
                    source,
                    value);
            }
        }
    }
}