var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
define(["require", "exports", "../FormActions", "../Mof", "../client/Actions", "../Settings", "../models/DatenMeister.class", "../client/Items", "../models/DatenMeister.class", "../client/Forms"], function (require, exports, FormActions, Mof_1, ActionClient, Settings, DatenMeister_class_1, ItemClient, DatenMeisterModel, FormClient) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.loadModules = void 0;
    function loadModules() {
        FormActions.addModule(new WorkspaceExtentXmiCreateNavigateAction());
        FormActions.addModule(new WorkspaceExtentLoadOrCreateNavigateAction());
        FormActions.addModule(new WorkspaceExtentLoadOrCreateAction());
        FormActions.addModule(new WorkspaceExtentLoadOrCreateStep2Action());
        FormActions.addModule(new WorkspaceExtentXmiCreateAction());
    }
    exports.loadModules = loadModules;
    class WorkspaceExtentXmiCreateNavigateAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Workspace.Extent.Xmi.Create.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                const workspaceId = (_a = parameter === null || parameter === void 0 ? void 0 : parameter.get('workspaceId')) !== null && _a !== void 0 ? _a : "";
                document.location.href =
                    Settings.baseUrl + "ItemAction/Workspace.Extent.Xmi.Create?metaClass=" +
                        encodeURIComponent(DatenMeister_class_1._DatenMeister._ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri) +
                        "&workspaceId=" + encodeURIComponent(workspaceId);
            });
        }
    }
    class WorkspaceExtentLoadOrCreateNavigateAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Workspace.Extent.LoadOrCreate.Navigate");
        }
        execute(form, element, parameter, submitMethod) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                const workspaceId = (_a = p === null || p === void 0 ? void 0 : p.get('workspaceId')) !== null && _a !== void 0 ? _a : "";
                document.location.href =
                    Settings.baseUrl + "ItemAction/Workspace.Extent.LoadOrCreate?workspaceId=" + encodeURIComponent(workspaceId);
            });
        }
    }
    class WorkspaceExtentLoadOrCreateAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Workspace.Extent.LoadOrCreate");
            this.actionVerb = "Choose Extent Type";
        }
        loadForm() {
            return __awaiter(this, void 0, void 0, function* () {
                return yield FormClient.getForm("dm:///_internal/forms/internal#WorkspacesAndExtents.Extent.SelectType");
            });
        }
        execute(form, element, parameter, submitMethod) {
            var _a;
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                const workspaceIdParameter = (_a = p === null || p === void 0 ? void 0 : p.get('workspaceId')) !== null && _a !== void 0 ? _a : "";
                const extentType = yield ItemClient.getProperty("Data", element.uri, "extentType");
                if (extentType === null || extentType === undefined) {
                    alert('No Extent Type has been selected');
                }
                else {
                    document.location.href = Settings.baseUrl +
                        "ItemAction/Workspace.Extent.LoadOrCreate.Step2" +
                        "?metaclass=" + encodeURIComponent(extentType.uri) +
                        (workspaceIdParameter !== undefined
                            ? ("&workspaceId=" + encodeURIComponent(workspaceIdParameter))
                            : "");
                }
            });
        }
    }
    class WorkspaceExtentLoadOrCreateStep2Action extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Workspace.Extent.LoadOrCreate.Step2");
            this.actionVerb = "Create/Load Extent";
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const extentCreationParameter = new Mof_1.DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);
                const result = yield ActionClient.executeActionDirectly("Execute", {
                    parameter: extentCreationParameter
                });
                if (result.success !== true) {
                    alert('Extent was not created successfully:\r\n\r\r\n' + result.reason + "\r\n\r\n" + result.stackTrace);
                }
                else {
                    alert('Extent was created successfully');
                }
            });
        }
    }
    class WorkspaceExtentXmiCreateAction extends FormActions.ItemFormActionModuleBase {
        constructor() {
            super("Workspace.Extent.Xmi.Create");
            this.actionVerb = "Create Xmi Extent";
        }
        loadObject() {
            return __awaiter(this, void 0, void 0, function* () {
                let p = new URLSearchParams(window.location.search);
                const result = new Mof_1.DmObject();
                result.setMetaClassByUri("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");
                result.set("workspaceId", p.get('workspaceId'));
                return Promise.resolve(result);
            });
        }
        execute(form, element, parameter, submitMethod) {
            return __awaiter(this, void 0, void 0, function* () {
                const extentCreationParameter = new Mof_1.DmObject();
                extentCreationParameter.set('configuration', element);
                extentCreationParameter.setMetaClassByUri(DatenMeisterModel._DatenMeister._Actions.__LoadExtentAction_Uri);
                const result = yield ActionClient.executeActionDirectly("Execute", {
                    parameter: extentCreationParameter
                });
                if (result.success) {
                    document.location.href = Settings.baseUrl
                        + "ItemsOverview/" + encodeURIComponent(element.get("workspaceId")) +
                        "/" + encodeURIComponent(element.get("extentUri"));
                }
                else {
                    alert(result.reason);
                }
            });
        }
    }
});
//# sourceMappingURL=WorkspaceModules.js.map