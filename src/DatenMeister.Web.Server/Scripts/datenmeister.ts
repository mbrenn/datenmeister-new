/// <reference path="typings/jquery/jquery.d.ts" />

export namespace DatenMeister {

    interface Workspace {
        id: string;
        annotation: string;
    };

    export class WorkspaceLogic {
        loadAndSetWorkbenchs(container: JQuery): JQueryPromise<Array<Workspace>> {
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
        
        setContent(container: JQuery, data: Array<Workspace>) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var line = compiled(data[n]);
                container.append($(line));
            }
        }
    };


    export namespace GUI {
        export function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndSetWorkbenchs($("#container_workspace")).done(function (data) {
                    //alert('We succeeded');
                }).fail(function () {
                    //alert('We failed');
                });
            });
        }
    }
    
};
