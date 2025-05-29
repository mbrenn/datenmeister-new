using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;

namespace DatenMeister.Actions.ActionHandler;

public class MoveUpDownActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(
            _DatenMeister.TheOne.Actions.__MoveUpDownAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        await Task.Run(() =>
        {
            var direction = action.getOrDefault<_DatenMeister._Actions.___MoveDirectionType>(
                _DatenMeister._Actions._MoveUpDownAction.direction);
            var element = action.getOrDefault<IElement>(
                              _DatenMeister._Actions._MoveUpDownAction.element)
                          ?? throw new InvalidOperationException("element is null");
            var property = action.getOrDefault<string>(
                _DatenMeister._Actions._MoveUpDownAction.property);
            var container = action.getOrDefault<IObject>(
                                _DatenMeister._Actions._MoveUpDownAction.container)
                            ?? throw new InvalidOperationException("container is null");

            IReflectiveSequence collection;
            if (container is IExtent extent)
            {
                collection = extent.elements();
            }
            else
            {
                collection = container.get<IReflectiveSequence>(property);
            }

            switch (direction)
            {
                case _DatenMeister._Actions.___MoveDirectionType.Up:
                    if (!CollectionHelper.MoveElementUp(collection, element))
                    {
                        throw new InvalidOperationException("Element was not found");
                    }

                    break;
                case _DatenMeister._Actions.___MoveDirectionType.Down:
                    if (!CollectionHelper.MoveElementDown(collection, element))
                    {
                        throw new InvalidOperationException("Element was not found");
                    }

                    break;
                default:
                    throw new InvalidOperationException("direction is not known: " + direction);
            }
        });

        return null;
    }
}