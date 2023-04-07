define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.getObjectFormFactory = exports.getCollectionFormFactory = exports.registerObjectForm = exports.registerCollectionForm = void 0;
    const registerDataCollectionForm = new Array();
    const registerDataObjectForm = new Array();
    function registerCollectionForm(uri, factoryFunction) {
        registerDataCollectionForm.push({
            uri: uri,
            factoryFunction: factoryFunction
        });
    }
    exports.registerCollectionForm = registerCollectionForm;
    function registerObjectForm(uri, factoryFunction) {
        registerDataObjectForm.push({
            uri: uri,
            factoryFunction: factoryFunction
        });
    }
    exports.registerObjectForm = registerObjectForm;
    function getCollectionFormFactory(uri) {
        for (let n in registerDataCollectionForm) {
            const item = registerDataCollectionForm[n];
            if (item.uri === uri) {
                return item.factoryFunction;
            }
        }
        return undefined;
    }
    exports.getCollectionFormFactory = getCollectionFormFactory;
    function getObjectFormFactory(uri) {
        for (let n in registerDataObjectForm) {
            const item = registerDataObjectForm[n];
            if (item.uri === uri) {
                return item.factoryFunction;
            }
        }
        return undefined;
    }
    exports.getObjectFormFactory = getObjectFormFactory;
});
//# sourceMappingURL=FormFactory.js.map