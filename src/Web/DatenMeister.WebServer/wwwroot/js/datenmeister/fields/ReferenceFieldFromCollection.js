var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Interfaces", "../Mof", "../client/Items", "../models/DatenMeister.class"], function (require, exports, Interfaces_1, Mof_1, ClientItems, DatenMeister_class_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Field = void 0;
    class Field extends Interfaces_1.BaseField {
        createDom(dmElement) {
            var _a, _b;
            return __awaiter(this, void 0, void 0, function* () {
                this._element = dmElement;
                const fieldName = (_b = (_a = this.field.get('name')) === null || _a === void 0 ? void 0 : _a.toString()) !== null && _b !== void 0 ? _b : "";
                if (this.isReadOnly) {
                    let value = dmElement.get(fieldName, Mof_1.ObjectType.Object);
                    // Checks, if there is a value set at all? 
                    if (value === undefined) {
                        return $("<em>Not set</em>");
                    }
                    // If yes, get the value
                    const textValue = yield ClientItems.getItemWithNameAndId(value.workspace, value.uri);
                    const result = $("<span></span>");
                    result.text(textValue.name);
                    return result;
                }
                else {
                    let value = dmElement.get(fieldName, Mof_1.ObjectType.Object);
                    this._dropDown = $("<select></select>");
                    let anySelected = false;
                    const notSelected = $("<option value=''>--- No selection ---</option>");
                    this._dropDown.append(notSelected);
                    const values = yield this.loadValuesFromServer();
                    for (const n in values) {
                        const item = values[n];
                        const option = $("<option></option>");
                        option.attr('value', item.uri);
                        if (value !== undefined && value.uri === item.uri) {
                            option.attr('selected', 'selected');
                            anySelected = true;
                        }
                        option.text(item.name);
                        this._dropDown.append(option);
                    }
                    if (!anySelected) {
                        notSelected.attr('selected', 'selected');
                    }
                    return this._dropDown;
                }
            });
        }
        evaluateDom(dmElement) {
            return __awaiter(this, void 0, void 0, function* () {
                const fieldName = this.field.get('name').toString();
                const fieldValue = this._dropDown.val();
                if (fieldValue === '' || fieldValue === undefined) {
                    dmElement.unset(fieldName);
                }
                else {
                    dmElement.set(fieldName, Mof_1.DmObject.createFromReference(this.field.get(DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace), fieldValue.toString()));
                }
            });
        }
        loadValuesFromServer() {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                if (this._element === undefined) {
                    throw "The element is not set. 'createDom' must be called in advance";
                }
                return yield ClientItems.getRootElementsAsItem((_a = this.field.get(DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, Mof_1.ObjectType.String)) !== null && _a !== void 0 ? _a : "", this.field.get(DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.collection, Mof_1.ObjectType.String));
            });
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ReferenceFieldFromCollection.js.map