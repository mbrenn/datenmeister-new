var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../forms/ActionForm", "../modules/DefaultLoader"], function (require, exports, Form, DefaultLoader_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.init = void 0;
    function init(actionName, metaClass, formUri) {
        return __awaiter(this, void 0, void 0, function* () {
            (0, DefaultLoader_1.loadDefaultModules)();
            yield Form.createActionFormForEmptyObject($("#form_view"), metaClass, { isReadOnly: false, allowAddingNewProperties: true, formUri: formUri }, actionName);
        });
    }
    exports.init = init;
});
//# sourceMappingURL=ItemAction.js.map