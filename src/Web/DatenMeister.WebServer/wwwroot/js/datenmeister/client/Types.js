var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../Settings", "../ApiConnection"], function (require, exports, Settings, ApiConnection) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getPropertyType = exports.getAllTypes = void 0;
    function getAllTypes() {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.get(Settings.baseUrl + "api/types/all");
        });
    }
    exports.getAllTypes = getAllTypes;
    /**
     * Gets the type of the property by referring to one metaClass and the propertyName
     * @param metaClass Uri of the metaClass to be queried
     * @param propertyName
     */
    function getPropertyType(workspace, metaClass, propertyName) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield ApiConnection.get(Settings.baseUrl + "api/types/propertytype/"
                + encodeURIComponent(workspace) + "/"
                + encodeURIComponent(metaClass) + "/"
                + encodeURIComponent(propertyName));
        });
    }
    exports.getPropertyType = getPropertyType;
});
//# sourceMappingURL=Types.js.map