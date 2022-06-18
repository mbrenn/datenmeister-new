var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Settings", "../ApiConnection", "../Mof"], function (require, exports, Settings, ApiConnection, Mof) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.executeAction = void 0;
    function executeAction(actionName, parameter) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/action/" +
                encodeURIComponent(actionName);
            return yield ApiConnection.post(url, { parameter: Mof.createJsonFromObject(parameter.parameter) });
        });
    }
    exports.executeAction = executeAction;
});
//# sourceMappingURL=Actions.js.map