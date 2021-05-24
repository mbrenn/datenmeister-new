var X = /** @class */ (function () {
    function X() {
    }
    X.prototype.y = function () {
    };
    return X;
}());
var Settings;
(function (Settings) {
    Settings.baseUrl = "/";
})(Settings || (Settings = {}));
var NameLoader = /** @class */ (function () {
    function NameLoader() {
    }
    NameLoader.loadNameOf = function (elementPosition) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURI(elementPosition.workspace) + "/" +
            encodeURI(elementPosition.extentUri) + "/" +
            encodeURI(elementPosition.item));
    };
    NameLoader.loadNameByUri = function (elementUri) {
        return $.ajax(Settings.baseUrl +
            "api/elements/get_name/" +
            encodeURI(elementUri));
    };
    return NameLoader;
}());
var DatenMeister;
(function (DatenMeister) {
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
