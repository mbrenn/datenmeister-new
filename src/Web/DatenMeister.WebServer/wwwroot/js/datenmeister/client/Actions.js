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
    exports.executeAction = exports.executeActionDirectly = void 0;
    function executeActionDirectly(actionName, parameter) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/action/execute_directly/" +
                encodeURIComponent(actionName);
            return yield ApiConnection.post(url, { parameter: Mof.createJsonFromObject(parameter.parameter) });
        });
    }
    exports.executeActionDirectly = executeActionDirectly;
    function executeAction(workspaceId, itemUri) {
        return __awaiter(this, void 0, void 0, function* () {
            let url = Settings.baseUrl +
                "api/action/execute/" +
                encodeURIComponent(workspaceId) + "/" +
                encodeURIComponent(itemUri);
            return yield ApiConnection.post(url, {});
        });
    }
    exports.executeAction = executeAction;
});
//# sourceMappingURL=Actions.js.map