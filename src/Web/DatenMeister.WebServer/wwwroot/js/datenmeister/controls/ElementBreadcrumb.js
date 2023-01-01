var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../client/Items", "../ApiModels", "../Navigator"], function (require, exports, ClientItems, ApiModels_1, Navigator) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ElementBreadcrumb = void 0;
    class ElementBreadcrumb {
        constructor(container) {
            this._container = container;
        }
        createForExtent(workspace, extentUri) {
            return __awaiter(this, void 0, void 0, function* () {
                const container = yield ClientItems.getContainer(workspace, extentUri, true);
                this.build(container);
            });
        }
        createForItem(workspace, itemUri) {
            return __awaiter(this, void 0, void 0, function* () {
                const container = yield ClientItems.getContainer(workspace, itemUri, true);
                this.build(container);
            });
        }
        build(container) {
            this._container.empty();
            let first = true;
            for (const n in container.reverse()) {
                // Creates the intersections
                if (!first) {
                    this._container.append($("<span> &lt; </span>"));
                }
                // Finds the item
                const item = container[n];
                const element = $("<a></a>");
                element.text(item.name);
                if (item.ententType === ApiModels_1.EntentType.Extent) {
                    element.attr('href', Navigator.getLinkForNavigateToExtent(item.workspace, item.extentUri));
                }
                else {
                    element.attr('href', Navigator.getLinkForNavigateToItemByUrl(item.workspace, item.uri));
                }
                this._container.append(element);
                first = false;
            }
        }
    }
    exports.ElementBreadcrumb = ElementBreadcrumb;
});
//# sourceMappingURL=ElementBreadcrumb.js.map