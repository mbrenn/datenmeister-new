/// <reference path="typings/jquery/jquery.d.ts" />

module DatenMeister {

    interface Workspace {
        id: string;
        annotation: string;
    };

    interface Extent {
        url: string;
    }

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
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {                        
                        return function () {
                            location.href = "/Home/workspace?id=" + localEntry.id;
                        };
                    } (entry)));

                container.append(dom);
            }
        }
    };

    export class ExtentLogic {
        loadAndSetExtents(container: JQuery, id: string): JQueryPromise<Object> {
            var tthis = this;

            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/all?id=" + id).
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
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {
                        return function () {
                            location.href = "/Home/extent?id=" + localEntry.uri;
                        };
                    } (entry)));

                container.append(dom);
            }
        }
    }

    export namespace GUI {
        export function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndSetWorkbenchs($("#container_workspace")).done(function (data) {
                }).fail(function () {
                });
            });
        }

        export function loadExtents(workspaceId) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndSetExtents($("#container_extents"), workspaceId).done(function (data) {
                }).fail(function () {
                });
            });
        }
    }    
};
