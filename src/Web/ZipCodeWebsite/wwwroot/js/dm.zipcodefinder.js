/// <reference path="../../node_modules/@types/jquery/index.d.ts" />
$(function () {
    $("#zipcode_searchtext").on({
        keyup: function () {
            var searchText = $("#zipcode_searchtext").val().toString();
            ZipCodeLoader.injectZipCodes(searchText);
        }
    });
    ZipCodeLoader.injectZipCodes();
});
var ZipCodeLoader = /** @class */ (function () {
    function ZipCodeLoader() {
    }
    ZipCodeLoader.getZipCodes = function (searchText) {
        var uri = "/zipcode/";
        if (searchText && 0 !== searchText.length) {
            uri += "?search=" + encodeURIComponent(searchText);
        }
        return $.ajax(uri);
    };
    ZipCodeLoader.injectZipCodes = function (searchText) {
        this.step++;
        var currentStep = this.step;
        var tthis = this;
        this.getZipCodes(searchText).done(function (data) {
            if (tthis.step != currentStep)
                return;
            var htmlResult = "<table class=\"table\">";
            htmlResult += "<tr><th>Zipcode</th><th>Name</th><th>Longitudinal</th><th>Latitude</th></tr>";
            for (var key in data.items) {
                if (!data.items.hasOwnProperty(key)) {
                    continue;
                }
                var value = data.items[key];
                htmlResult += "<tr>" +
                    "<td>" + value.zip + "</td>" +
                    "<td>" + value.name + "</td>" +
                    "<td>" + Math.round(value.positionLong * 1000) / 1000 + "</td>" +
                    "<td>" + Math.round(value.positionLat * 1000) / 1000 + "</td>" +
                    "</tr>";
            }
            htmlResult += "</table>";
            $("#zipcode_resulttable").html(htmlResult);
        });
    };
    ZipCodeLoader.step = 0;
    return ZipCodeLoader;
}());
//# sourceMappingURL=dm.zipcodefinder.js.map