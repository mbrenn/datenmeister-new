/// <reference path="node_modules/@types/jquery/index.d.ts" />
$(function () {
    $("#zipcode_searchtext").on({
        keyup: function () {
            alert($("#zipcode_searchtext").val());
        }
    });
    ZipCodeLoader.injectZipCodes();
});
;
;
var ZipCodeLoader = /** @class */ (function () {
    function ZipCodeLoader() {
    }
    ZipCodeLoader.getZipCodes = function (searchText) {
        var uri = "/zipcode/";
        if (!searchText || 0 === searchText.length) {
            uri += "?search=" + encodeURIComponent(searchText);
        }
        return $.ajax(uri);
    };
    ZipCodeLoader.injectZipCodes = function (searchText) {
        this.getZipCodes(searchText).done(function (data) {
            var htmlResult = "<table>";
            for (var key in data.items) {
                if (!data.items.hasOwnProperty(key)) {
                    continue;
                }
                var value = data.items[key];
                htmlResult += "<tr>" +
                    "<td>" + value.zip + "</td>" +
                    "<td>" + value.name + "</td>" +
                    "<td>" + value.positionLong + "</td>" +
                    "<td>" + value.positionLat + "</td>" +
                    "</tr>";
            }
            htmlResult += "</table>";
            $("#zipcode_resulttable").html(htmlResult);
        });
    };
    return ZipCodeLoader;
}());
//# sourceMappingURL=dm.zipcodefinder.js.map