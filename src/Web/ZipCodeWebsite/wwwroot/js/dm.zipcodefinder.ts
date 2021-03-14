/// <reference path="node_modules/@types/jquery/index.d.ts" />

$(
    function() {
        $("#zipcode_searchtext").on({
            keyup: function () {
                alert($("#zipcode_searchtext").val());
            }
        });
        
        ZipCodeLoader.injectZipCodes();
    });

interface ZipCodeData {
    items: Array<ZipCode>;
};

interface ZipCode{
    id: number;
    name: string;
    zip: number;
    positionLong: number;
    positionLat: number;
};

class ZipCodeLoader {
    constructor() {
    }

    static getZipCodes(searchText?: string): JQuery.jqXHR {
        var uri = "/zipcode/";
        
        if(!searchText || 0 === searchText.length)
        {
            uri += "?search=" + encodeURIComponent(searchText);
        }
        
        return $.ajax(uri);
    }
    
    static injectZipCodes(searchText?: string): void {
        this.getZipCodes(searchText).done(
            function (data: ZipCodeData) {
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
            }
        );
    }
}