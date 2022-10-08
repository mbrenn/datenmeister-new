var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Actions", "../models/DatenMeister.class", "../Mof"], function (require, exports, Actions, _DatenMeister, Mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.moveItemInExtentUp = exports.moveItemInExtentDown = exports.moveItemInCollectionUp = exports.moveItemInCollectionDown = void 0;
    var _MoveUpDownAction = _DatenMeister._DatenMeister._Actions._MoveUpDownAction;
    var _MoveDirectionType = _DatenMeister._DatenMeister._Actions._MoveDirectionType;
    function moveItemInCollection(workspace, parentUri, propertyName, elementUri, direction) {
        return __awaiter(this, void 0, void 0, function* () {
            const action = new Mof.DmObject(_DatenMeister._DatenMeister._Actions.__MoveUpDownAction_Uri);
            action.set(_MoveUpDownAction.container, Mof.DmObject.createFromReference(workspace, parentUri));
            action.set(_MoveUpDownAction.property, propertyName);
            action.set(_MoveUpDownAction.element, Mof.DmObject.createFromReference(workspace, elementUri));
            action.set(_MoveUpDownAction.direction, direction);
            const parameter = {
                parameter: action
            };
            yield Actions.executeActionDirectly("Execute", parameter);
        });
    }
    function moveItemInCollectionDown(workspace, parentUri, propertyName, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            yield moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Down);
        });
    }
    exports.moveItemInCollectionDown = moveItemInCollectionDown;
    function moveItemInCollectionUp(workspace, parentUri, propertyName, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            yield moveItemInCollection(workspace, parentUri, propertyName, elementUri, _MoveDirectionType.Up);
        });
    }
    exports.moveItemInCollectionUp = moveItemInCollectionUp;
    function moveItemInExtentDown(workspace, extentUri, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            yield moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Down);
        });
    }
    exports.moveItemInExtentDown = moveItemInExtentDown;
    function moveItemInExtentUp(workspace, extentUri, elementUri) {
        return __awaiter(this, void 0, void 0, function* () {
            yield moveItemInCollection(workspace, extentUri, undefined, elementUri, _MoveDirectionType.Up);
        });
    }
    exports.moveItemInExtentUp = moveItemInExtentUp;
});
//# sourceMappingURL=Actions.Items.js.map