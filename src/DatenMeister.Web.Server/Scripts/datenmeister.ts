/// <reference path="typings/jquery/jquery.d.ts" />

namespace DatenMeister {

    interface Workbench {
    };

    export class WorkbenchLogic {
        loadWorkbenchs(): JQueryPromise<Array<Workbench>> {

            var callback = $.Deferred();
            $.ajax("/api/datenmeister/workspace/all").
                done(function (data) {
                    callback.resolve(null);
                })
                .fail(function (data) {
                    callback.reject(null);
                });

            return callback;
        }
    };
};

$(document).ready(function () {
    var workbenchLogic = new DatenMeister.WorkbenchLogic();
    workbenchLogic.loadWorkbenchs().done(function (data) {
        alert('We succeeded');
    }).fail(function () {
        alert('We failed');
    });
});