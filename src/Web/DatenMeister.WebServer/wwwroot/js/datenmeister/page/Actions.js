var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../controls/SelectItemControl"], function (require, exports, SIC) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.pageOpenSelectItemControlWithItem = exports.pageOpenSelectItemControlWithExtent = exports.pageOpenSelectItemControlWithWorkspace = exports.pageOpenSelectItemControlFullBreadcrumb = exports.pageOpenSelectItemControl = void 0;
    function pageOpenSelectItemControl() {
        return __awaiter(this, void 0, void 0, function* () {
            const selectItemControl = $("#selectitemcontrol");
            selectItemControl.empty();
            const control = new SIC.SelectItemControl();
            control.itemSelected.addListener(x => alert("Uri:" + x.uri + ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
            yield control.initAsync(selectItemControl);
        });
    }
    exports.pageOpenSelectItemControl = pageOpenSelectItemControl;
    function pageOpenSelectItemControlFullBreadcrumb() {
        return __awaiter(this, void 0, void 0, function* () {
            const selectItemControl = $("#selectitemcontrol");
            selectItemControl.empty();
            const control = new SIC.SelectItemControl();
            control.itemSelected.addListener(x => alert("Uri:" + x.uri + ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
            const settings = new SIC.Settings();
            settings.showExtentInBreadcrumb = true;
            settings.showWorkspaceInBreadcrumb = true;
            yield control.initAsync(selectItemControl, settings);
        });
    }
    exports.pageOpenSelectItemControlFullBreadcrumb = pageOpenSelectItemControlFullBreadcrumb;
    function pageOpenSelectItemControlWithWorkspace() {
        return __awaiter(this, void 0, void 0, function* () {
            const selectItemControl = $("#selectitemcontrol");
            selectItemControl.empty();
            const control = new SIC.SelectItemControl();
            control.itemSelected.addListener(x => alert("Uri:" + x.uri + ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
            yield control.setWorkspaceById("Types");
            yield control.initAsync(selectItemControl);
        });
    }
    exports.pageOpenSelectItemControlWithWorkspace = pageOpenSelectItemControlWithWorkspace;
    function pageOpenSelectItemControlWithExtent() {
        return __awaiter(this, void 0, void 0, function* () {
            const selectItemControl = $("#selectitemcontrol");
            selectItemControl.empty();
            const control = new SIC.SelectItemControl();
            control.itemSelected.addListener(x => alert("Uri:" + x.uri + ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
            yield control.setExtentByUri("Types", "dm:///_internal/types/internal");
            yield control.initAsync(selectItemControl);
        });
    }
    exports.pageOpenSelectItemControlWithExtent = pageOpenSelectItemControlWithExtent;
    function pageOpenSelectItemControlWithItem() {
        return __awaiter(this, void 0, void 0, function* () {
            const selectItemControl = $("#selectitemcontrol");
            selectItemControl.empty();
            const control = new SIC.SelectItemControl();
            control.itemSelected.addListener(x => alert("Uri:" + x.uri + ", Extent:" + x.extentUri + ", Workspace: " + x.workspace));
            yield control.setItemByUri("Types", "dm:///_internal/types/internal#DatenMeister.Modules.ZipCodeExample.Model.ZipCode");
            yield control.initAsync(selectItemControl);
        });
    }
    exports.pageOpenSelectItemControlWithItem = pageOpenSelectItemControlWithItem;
});
//# sourceMappingURL=Actions.js.map