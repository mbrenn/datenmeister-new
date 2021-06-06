var Settings;
(function (Settings) {
    Settings.baseUrl = "/";
})(Settings || (Settings = {}));
var ApiConnection;
(function (ApiConnection) {
    function post(uri, data) {
        return $.ajax({
            url: uri,
            data: JSON.stringify(data),
            dataType: "json",
            contentType: "application/json",
            method: "POST"
        });
    }
    ApiConnection.post = post;
})(ApiConnection || (ApiConnection = {}));
var NameLoader = /** @class */ (function () {
    function NameLoader() {
    }
    NameLoader.loadNameOf = function (elementPosition) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURIComponent(elementPosition.workspace) + "/" +
            encodeURIComponent(elementPosition.extentUri) + "/" +
            encodeURIComponent(elementPosition.item));
    };
    NameLoader.loadNameByUri = function (elementUri) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURIComponent(elementUri));
    };
    return NameLoader;
}());
var DatenMeister;
(function (DatenMeister) {
    var FormActions = /** @class */ (function () {
        function FormActions() {
        }
        FormActions.extentNavigateTo = function (workspace, extentUri) {
            document.location.href = Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
        };
        FormActions.createZipExample = function (workspace) {
            ApiConnection.post(Settings.baseUrl + "api/zip/create", { workspace: workspace })
                .done(function (data) {
                document.location.reload();
            });
        };
        FormActions.createItem = function (workspace, extentUri) {
            ApiConnection.post(Settings.baseUrl + "api/items/create", {
                workspace: workspace,
                extentUri: extentUri
            })
                .done(function (data) {
                document.location.reload();
            });
        };
        FormActions.deleteItem = function (workspace, extentUri, itemId) {
            ApiConnection.post(Settings.baseUrl + "api/items/delete", {
                workspace: workspace,
                extentUri: extentUri,
                item: itemId
            })
                .done(function (data) {
                Navigator.navigateToExtent(workspace, extentUri);
            });
        };
        return FormActions;
    }());
    DatenMeister.FormActions = FormActions;
    var Navigator = /** @class */ (function () {
        function Navigator() {
        }
        Navigator.navigateToWorkspaces = function () {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/Management/dm:%2F%2F%2F_internal%2Fworkspaces";
        };
        Navigator.navigateToWorkspace = function (workspace) {
            document.location.href =
                Settings.baseUrl + "Item/Management/dm%3A%2F%2F%2F_internal%2Fworkspaces/" +
                    encodeURIComponent(workspace);
        };
        Navigator.navigateToExtent = function (workspace, extentUri) {
            document.location.href =
                Settings.baseUrl + "ItemsOverview/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri);
        };
        Navigator.navigateToItem = function (workspace, extentUri, itemId) {
            document.location.href =
                Settings.baseUrl + "Item/" +
                    encodeURIComponent(workspace) + "/" +
                    encodeURIComponent(extentUri) + "/" +
                    encodeURIComponent(itemId);
        };
        return Navigator;
    }());
    DatenMeister.Navigator = Navigator;
    var DomHelper = /** @class */ (function () {
        function DomHelper() {
        }
        DomHelper.injectName = function (domElement, elementPosition) {
            NameLoader.loadNameOf(elementPosition).done(function (x) {
                domElement.text(x.name);
            });
        };
        DomHelper.injectNameByUri = function (domElement, elementUri) {
            NameLoader.loadNameByUri(elementUri).done(function (x) {
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
        };
        return DomHelper;
    }());
    DatenMeister.DomHelper = DomHelper;
})(DatenMeister || (DatenMeister = {}));
