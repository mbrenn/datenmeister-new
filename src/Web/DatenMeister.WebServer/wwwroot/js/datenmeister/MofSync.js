var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "./Mof", "./client/Items", "./client/Elements", "./Mof"], function (require, exports, Mof, ClientItem, ClientElements, Mof_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.sync = exports.createTemporaryDmObject = void 0;
    /**
     * Creates a temporary DmObjectWithSync which is mirrored on the server
     * @param metaClass Metaclass of the element to be created
     */
    function createTemporaryDmObject(metaClass) {
        return __awaiter(this, void 0, void 0, function* () {
            const result = yield ClientElements.createTemporaryElement(metaClass);
            return Mof_1.DmObjectWithSync.createFromReference(result.workspace, result.uri);
        });
    }
    exports.createTemporaryDmObject = createTemporaryDmObject;
    /**
     * Performs a sync of the element to the server.
     * Only changes of the element are synced with the server
     * @param element Element ot be synced
     */
    function sync(element) {
        var _a;
        return __awaiter(this, void 0, void 0, function* () {
            // Check first the set elements
            const paras = new Array();
            for (const key in element.propertiesSet) {
                const value = element.get(key, Mof.ObjectType.Default);
                if (value === undefined || value === null) {
                    // Element is not set, so unset it
                    yield ClientItem.unsetProperty(element.workspace, element.uri, key);
                    console.log('MofSync: Unsetting: ' + element.uri + " - " + key);
                }
                else if ((typeof value === "object" || value === "function") && (value !== null)) {
                    // Element is a reference, so we need to set the reference directly
                    const referenceValue = value;
                    yield ClientItem.setPropertyReference(element.workspace, element.uri, {
                        property: key,
                        workspaceId: referenceValue.workspace,
                        referenceUri: referenceValue.uri
                    });
                    console.log('MofSync: Setting Reference for: ' + element.uri + " - " + key);
                }
                else {
                    // Element is a pure property           
                    paras.push({
                        key: key,
                        value: (_a = value === null || value === void 0 ? void 0 : value.toString()) !== null && _a !== void 0 ? _a : ""
                    });
                    console.log('MofSync: Setting: ' + element.uri + " - " + key);
                }
            }
            // Checks, if there is any property to be set
            if (paras.length > 0) {
                // Ok, we need to set the properties
                yield ClientItem.setPropertiesByStringValues(element.workspace, element.uri, {
                    properties: paras
                });
            }
            element.clearSync();
        });
    }
    exports.sync = sync;
});
//# sourceMappingURL=MofSync.js.map