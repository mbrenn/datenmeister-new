
/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="d.ts/underscore/underscore.d.ts" />

interface ZipCodeData {
    Zipcode: string;
    City: String;
};

class ZipCode {
    init() {
        var tthis = this;
        $("#zipcode_source").on('input', function () {
            
            var zipValue = $("#zipcode_source").val();
            
            $.ajax("api/zip?zip=" + encodeURI(zipValue)).done(function (data) {
                tthis.showZipCodes(data);
            });

        });
    }

    showZipCodes(data: Array<ZipCodeData>) {
        $(".zipcoderesult").empty();
        var compiled = _.template($("#zip_template").html());
        for (var n in data) {
            var line = compiled(data[n]);
            $(".zipcoderesult").append($(line));
        }
    }
}
