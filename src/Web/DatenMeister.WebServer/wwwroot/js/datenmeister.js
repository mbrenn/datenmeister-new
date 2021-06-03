// eslint-disable-next-line @typescript-eslint/no-namespace
// eslint-disable-next-line @typescript-eslint/prefer-namespace-keyword
var Settings;
(function (Settings) {
    Settings.baseUrl = "/";
})(Settings || (Settings = {}));
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
var DatenMeister;
(function (DatenMeister) {
    class FormActions {
        static extentNavigateTo(workspace, extentUri) {
            document.location.href = Settings.baseUrl + "ItemsOverview/" +
                encodeURIComponent(workspace) + "/" +
                encodeURIComponent(extentUri);
        }
    }
    DatenMeister.FormActions = FormActions;
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
})(DatenMeister || (DatenMeister = {}));
//# sourceMappingURL=datenmeister.js.map