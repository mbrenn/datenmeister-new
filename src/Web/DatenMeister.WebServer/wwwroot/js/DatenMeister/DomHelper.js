var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    Object.defineProperty(o, k2, { enumerable: true, get: function() { return m[k]; } });
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
define(["require", "exports", "./NameLoader"], function (require, exports, NameLoader) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.injectNameByUri = exports.injectName = void 0;
    NameLoader = __importStar(NameLoader);
    function injectName(domElement, elementPosition) {
        NameLoader.loadNameOf(elementPosition).done(x => {
            domElement.text(x.name);
        });
    }
    exports.injectName = injectName;
    function injectNameByUri(domElement, elementUri) {
        NameLoader.loadNameByUri(elementUri).done(x => {
            if (x.extentUri !== undefined && x.workspace !== undefined
                && x.extentUri !== "" && x.workspace !== ""
                && x.itemId !== "" && x.itemId !== undefined) {
                const linkElement = $("<a></a>");
                linkElement.text(x.name);
                linkElement.attr("href", "/Item/" + encodeURIComponent(x.workspace) +
                    "/" + encodeURIComponent(x.extentUri) +
                    "/" + encodeURIComponent(x.itemId));
                domElement.empty();
                domElement.append(linkElement);
            }
            else {
                domElement.text(x.name);
            }
        });
    }
    exports.injectNameByUri = injectNameByUri;
});
//# sourceMappingURL=DomHelper.js.map