import * as Actions from './Actions';
import * as _DatenMeister from '../models/DatenMeister.class';
import * as Mof from '../Mof';
import _MoveUpDownAction = _DatenMeister._DatenMeister._Actions._MoveUpDownAction;
import _MoveDirectionType = _DatenMeister._DatenMeister._Actions._MoveDirectionType;

async function moveItemInCollection(workspace: string, parentUri: string, propertyName: string | undefined, elementUri: string, direction: string) {
    const action = new Mof.DmObject(_DatenMeister._DatenMeister._Actions.__MoveUpDownAction_Uri);
    action.set(_MoveUpDownAction.container, Mof.DmObject.createFromReference(workspace, parentUri));
    action.set(_MoveUpDownAction.property, propertyName);
    action.set(_MoveUpDownAction.element, Mof.DmObject.createFromReference(workspace, elementUri));
    action.set(_MoveUpDownAction.direction, direction);

    const parameter: Actions.ExecuteActionParams =
        {
            parameter: action
        };

    await Actions.executeActionDirectly("Execute", parameter);
}

export async function moveItemInCollectionDown(workspace: string, parentUri: string, propertyName: string, elementUri: string) {

    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Down);
}

export async function moveItemInCollectionUp(workspace: string, parentUri: string, propertyName: string, elementUri: string) {

    await moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Up);
}

export async function moveItemInExtentDown(workspace: string, extentUri: string, elementUri: string) {

    await moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Down);
}

export async function moveItemInExtentUp(workspace: string, extentUri: string, elementUri: string) {

    await moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Up);
}