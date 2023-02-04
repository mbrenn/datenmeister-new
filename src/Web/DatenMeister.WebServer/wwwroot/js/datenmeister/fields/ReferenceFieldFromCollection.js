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
            return __awaiter(this, void 0, void 0, function* () {
                this._dropDown = $("<select></select>");
                const values = yield this.loadValuesFromServer();
                for (const n in values) {
                    const item = values[n];
                    const option = $("<option></option>");
                    option.attr('value', item.uri);
                    option.text(item.name);
                    this._dropDown.append(option);
                }
                return this._dropDown;
            });
        }
        evaluateDom(dmElement) {
            const fieldName = this.field.get('name').toString();
            dmElement.set(fieldName, Mof_1.DmObject.createFromReference(DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, this._dropDown.val().toString()));
        }
        loadValuesFromServer() {
            return __awaiter(this, void 0, void 0, function* () {
                return yield ClientItems.getRootElements(DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, DatenMeister_class_1._DatenMeister._Forms._ReferenceFieldFromCollectionData.collection);
            });
        }
    }
    exports.Field = Field;
});
//# sourceMappingURL=ReferenceFieldFromCollection.js.map