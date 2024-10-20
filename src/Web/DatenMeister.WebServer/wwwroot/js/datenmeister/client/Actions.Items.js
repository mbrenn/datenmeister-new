import * as Actions from './Actions.js';
import * as _DatenMeister from '../models/DatenMeister.class.js';
import * as Mof from '../Mof.js';
var _MoveUpDownAction = _DatenMeister._DatenMeister._Actions._MoveUpDownAction;
var _MoveDirectionType = _DatenMeister._DatenMeister._Actions._MoveDirectionType;
async function moveItemInCollection(workspace, parentUri, propertyName, elementUri, direction) {
    const action = new Mof.DmObject(_DatenMeister._DatenMeister._Actions.__MoveUpDownAction_Uri);
    action.set(_MoveUpDownAction.container, Mof.DmObject.createFromReference(workspace, parentUri));
    action.set(_MoveUpDownAction.property, propertyName);
    action.set(_MoveUpDownAction.element, Mof.DmObject.createFromReference(workspace, elementUri));
    action.set(_MoveUpDownAction.direction, direction);
    const parameter = {
        parameter: action
    };
    await Actions.executeActionDirectly("Execute", parameter);
}
export async function moveItemInCollectionDown(workspace, parentUri, propertyName, elementUri) {
    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Down);
}
export async function moveItemInCollectionUp(workspace, parentUri, propertyName, elementUri) {
    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Up);
}
export async function moveItemInExtentDown(workspace, extentUri, elementUri) {
    await moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Down);
}
export async function moveItemInExtentUp(workspace, extentUri, elementUri) {
    await moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Up);
}
//# sourceMappingURL=Actions.Items.js.map