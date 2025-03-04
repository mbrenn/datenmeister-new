﻿/// <reference path="../../node_modules/@types/jquery/index.d.ts" />

$(
    function() {
        $("#zipcode_searchtext").on({
            keyup: function () {
                var searchText = $("#zipcode_searchtext").val().toString();
                ZipCodeLoader.injectZipCodes(searchText);
            }
        });
        
        ZipCodeLoader.injectZipCodes();
    });

interface ZipCodeData {
    items: Array<ZipCode>;
    truncated: boolean;
    noItemFound: boolean;
}

interface ZipCode{
    id: number;
    name: string;
    zip: number;
    positionLong: number;
    positionLat: number;
}

class ZipCodeLoader {
    constructor() {
    }

    static getZipCodes(searchText?: string): JQuery.jqXHR<ZipCodeData> {
        let uri = "/zipcode/";
        if (searchText && 0 !== searchText.length) {
            uri += "?search=" + encodeURIComponent(searchText);
        }

        return $.ajax(uri);
    }

    static step: number = 0;

    static injectZipCodes(searchText?: string): void {

        this.step++;
        const currentStep = this.step;

        this.getZipCodes(searchText).done(
            (data: ZipCodeData) => {
                if (this.step !== currentStep) return;
                
                let htmlResult = "";
                if (data.truncated === true)
                {
                    htmlResult += '<div class="alert alert-primary" role="alert">More than 100 items found!</div>';
                }
                
                if (data.noItemFound === true)
                {
                    htmlResult += '<div class="alert alert-danger" role="alert">No item found!</div>';
                }
                
                htmlResult += "<table class=\"table\">";
                htmlResult += "<tr><th>Zipcode</th><th>Name</th><th>Longitudinal</th><th>Latitude</th></tr>"
                for (let key in data.items) {
                    if (!data.items.hasOwnProperty(key)) {
                        continue;
                    }

                    const value = data.items[key];
                    htmlResult += "<tr>" +
                        "<td>" + value.zip + "</td>" +
                        "<td>" + value.name + "</td>" +
                        "<td>" + Math.round(value.positionLong * 1000) / 1000 + "</td>" +
                        "<td>" + Math.round(value.positionLat * 1000) / 1000 + "</td>" +
                        "</tr>";
                }

                htmlResult += "</table>";
                $("#zipcode_resulttable").html(htmlResult);
            }
        );
    }
}