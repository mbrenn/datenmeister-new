﻿using System;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
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

        public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
        {
            return await Task.Run(() =>
            {
                var result = InMemoryObject.CreateEmpty();
                var source = action.getOrDefault<IObject>(_DatenMeister._Actions._MoveOrCopyAction.source)
                             ?? throw new InvalidOperationException("'Source' is not set.");
                var value = action.getOrDefault<IObject>(_DatenMeister._Actions._MoveOrCopyAction.target)
                            ?? throw new InvalidOperationException("'target' is not set");
                var actionType = action.getOrDefault<_DatenMeister._Actions.___MoveOrCopyType>(
                    _DatenMeister._Actions._MoveOrCopyAction.copyMode);
                
                // Copies the item
                if (actionType == _DatenMeister._Actions.___MoveOrCopyType.Copy)
                {
                    var resultItem = ObjectOperations.CopyObject(
                        source,
                        value);

                    var copyWorkspace = resultItem.GetExtentOf()?.GetWorkspace();
                    if (copyWorkspace != null)
                    {   
                        result.set(
                            _DatenMeister._Actions._MoveOrCopyActionResult.targetWorkspace,
                            copyWorkspace.id);
                    }
                    
                    result.set(_DatenMeister._Actions._MoveOrCopyActionResult.targetUrl,
                        resultItem.GetUri());
                }

                // Moves the item
                else if (actionType == _DatenMeister._Actions.___MoveOrCopyType.Move)
                {
                    var resultItem = ObjectOperations.MoveObject(
                        source,
                        value);
                    
                    var moveWorkspace = resultItem.GetExtentOf()?.GetWorkspace();
                    if (moveWorkspace != null)
                    {   
                        result.set(_DatenMeister._Actions._MoveOrCopyActionResult.targetWorkspace,
                            moveWorkspace.id);
                    }
                    
                    result.set(_DatenMeister._Actions._MoveOrCopyActionResult.targetUrl,
                        resultItem.GetUri());
                }
                else
                {
                    throw new InvalidOperationException("Unknown Action Type" + actionType);
                }

                return result;
            });
        }
    }
}