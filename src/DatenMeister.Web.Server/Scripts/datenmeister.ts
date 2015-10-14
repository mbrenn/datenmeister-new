/// <reference path="typings/jquery/jquery.d.ts" />

namespace DatenMeister {

    interface Workbench {
    };

    export class WorkbenchLogic {
        loadAndSetWorkbenchs(container: JQuery): JQueryPromise<Array<Workbench>> {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/workspace/all").
                done(function (data) {
                    tthis.setContent(container, data);
                    callback.resolve(null);
                })
                .fail(function (data) {
                    callback.reject(null);
                });

            return callback;
        }
        
        setContent(container: JQuery, data: Array<Workbench>) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var line = compiled(data[n]);
                container.append($(line));
            }
        }
    };
};

$(document).ready(function () {
    var workbenchLogic = new DatenMeister.WorkbenchLogic();
    workbenchLogic.loadAndSetWorkbenchs($("#container_workspace")).done(function (data) {
        //alert('We succeeded');
    }).fail(function () {
        //alert('We failed');
    });
});