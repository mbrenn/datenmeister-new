define(["require", "exports", "./DatenMeister/ApiConnection"], function (require, exports, ApiConnection_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.DatenMeister = exports.NameLoader = void 0;
    var Settings;
    (function (Settings) {
        Settings.baseUrl = "/";
    })(Settings || (Settings = {}));
    var Mof;
    (function (Mof) {
        class DmObject {
            constructor() {
                this._values = new Array();
            }
            set(key, value) {
                this._values[key] = value;
            }
            get(key) {
                return this._values[key];
            }
            isSet(key) {
                return this._values[key] !== undefined;
            }
            unset(key) {
                this._values[key] = undefined;
            }
        }
        Mof.DmObject = DmObject;
    })(Mof || (Mof = {}));
    class NameLoader {
        static loadNameOf(elementPosition) {
            return $.ajax(Settings.baseUrl +
                "api/elements/get_name/" +
                encodeURIComponent(elementPosition.workspace) + "/" +
                encodeURIComponent(elementPosition.extentUri) + "/" +
                encodeURIComponent(elementPosition.item));
        }
        static loadNameByUri(elementUri) {
            return $.ajax(Settings.baseUrl +
                "api/elements/get_name/" +
                encodeURIComponent(elementUri));
        }
    }
    exports.NameLoader = NameLoader;
    var DatenMeister;
    (function (DatenMeister) {
        class FormActions {
            static extentNavigateTo(workspace, extentUri) {
                document.location.href = Settings.baseUrl + "ItemsOverview/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri);
            }
            static createZipExample(workspace) {
                ApiConnection_1.ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: workspace })
                    .done(function (data) {
                    document.location.reload();
                });
            }
            static itemNew(workspace, extentUri) {
                ApiConnection_1.ApiConnection.post(Settings.baseUrl + "api/items/create", {
                    workspace: workspace,
                    extentUri: extentUri
                })
                    .done(function (data) {
                    document.location.reload();
                });
            }
            static itemDelete(workspace, extentUri, itemId) {
                ApiConnection_1.ApiConnection.post(Settings.baseUrl + "api/items/delete", {
                    workspace: workspace,
                    extentUri: extentUri,
                    itemId: itemId
                })
                    .done(function (data) {
                    Navigator.navigateToExtent(workspace, extentUri);
                });
            }
            static extentsListViewItem(workspace, extentUri, itemId) {
                document.location.href = Settings.baseUrl + "Item/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri) + "/" +
                    encodeURIComponent(itemId);
            }
            static extentsListDeleteItem(workspace, extentUri, itemId) {
                ApiConnection_1.ApiConnection.post(Settings.baseUrl + "api/items/delete_from_extent", {
                    workspace: workspace,
                    extentUri: extentUri,
                    itemId: itemId
                })
                    .done(function (data) {
                    document.location.reload();
                });
            }
        }
        DatenMeister.FormActions = FormActions;
        class Navigator {
            static navigateToWorkspaces() {
                document.location.href =
                    Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
            }
            static navigateToWorkspace(workspace) {
                document.location.href =
                    Settings.baseUrl + "Item/Management/dm%3A%2F%2F%2F_internal%2Fworkspaces/" +
                        encodeURIComponent(workspace);
            }
            static navigateToExtent(workspace, extentUri) {
                document.location.href =
                    Settings.baseUrl + "ItemsOverview/" +
                        encodeURIComponent(workspace) + "/" +
                        encodeURIComponent(extentUri);
            }
            static navigateToItem(workspace, extentUri, itemId) {
                document.location.href =
                    Settings.baseUrl + "Item/" +
                        encodeURIComponent(workspace) + "/" +
                        encodeURIComponent(extentUri) + "/" +
                        encodeURIComponent(itemId);
            }
        }
        DatenMeister.Navigator = Navigator;
        let Forms;
        (function (Forms) {
            class Form {
            }
            Forms.Form = Form;
            class TextField {
                createDom(parent, dmElement) {
                    var fieldName = this.Field['name'];
                    this._textBox = $("<input />");
                    this._textBox.val(dmElement.get(fieldName).toString());
                }
                evaluateDom(dmElement) {
                }
            }
            Forms.TextField = TextField;
        })(Forms = DatenMeister.Forms || (DatenMeister.Forms = {}));
        class DomHelper {
            static injectName(domElement, elementPosition) {
                NameLoader.loadNameOf(elementPosition).done(x => {
                    domElement.text(x.name);
                });
            }
            static injectNameByUri(domElement, elementUri) {
                NameLoader.loadNameByUri(elementUri).done(x => {
                    if (x.extentUri !== undefined && x.workspace !== undefined
                        && x.extentUri !== "" && x.workspace !== ""
                        && x.itemId !== "" && x.itemId !== undefined) {
                        var linkElement = $("<a></a>");
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
        }
        DatenMeister.DomHelper = DomHelper;
    })(DatenMeister = exports.DatenMeister || (exports.DatenMeister = {}));
});
//# sourceMappingURL=datenmeister.js.map