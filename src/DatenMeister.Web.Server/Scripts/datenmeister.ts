/// <reference path="typings/jquery/jquery.d.ts" />

module DatenMeister {

    interface Workspace {
        id: string;
        annotation: string;
    };

    interface Extent {
        url: string;
    }

    interface MofObject {
        values: Array<string>;
    }

    export class WorkspaceLogic {
        loadAndCreateHtmlForWorkbenchs(container: JQuery): JQueryPromise<Array<Workspace>> {
            var tthis = this;
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/workspace/all").
                done(function (data) {
                    tthis.createHtmlForWorkbenchs(container, data);
                    callback.resolve(null);
                })
                .fail(function (data) {
                    callback.reject(null);
                });

            return callback;
        }
        
        createHtmlForWorkbenchs(container: JQuery, data: Array<Workspace>) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {                        
                        return function () {
                            location.href = "/Home/workspace?ws=" + localEntry.id;
                        };
                    } (entry)));

                container.append(dom);
            }
        }
    };

    export class ExtentLogic {
        loadAndCreateHtmlForExtents(container: JQuery, ws: string): JQueryPromise<Object> {
            var tthis = this;

            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/all?ws=" + ws).
                done(function (data) {
                    tthis.createHtmlForExtent(container, ws, data);
                    callback.resolve(null);
                })
                .fail(function (data) {
                    callback.reject(null);
                });

            return callback;
        }

        createHtmlForExtent(container: JQuery, ws: string, data: Array<Workspace>) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click(
                    (function (localEntry) {
                        return function () {
                            location.href = "/Home/extent?ws=" + ws + "&extent=" + localEntry.uri;
                        };
                    } (entry)));

                container.append(dom);
            }
        }

        loadAndCreateHtmlForItems(container: JQuery, ws: string, extentUrl: string) {
            var tthis = this;

            var callback = $.Deferred();
            $.ajax("/api/datenmeister/extent/get?ws=" + ws + "&url=" + extentUrl).
                done(function (data) {
                    tthis.createHtmlForItems(container, ws, extentUrl, data);
                    callback.resolve(null);
                })
                .fail(function (data) {
                    callback.reject(null);
                });

            return callback;
        }

        createHtmlForItems(container: JQuery, ws: string, extentUrl: string, data: Array<MofObject>) {
            container.empty();
        }
    }

    export namespace GUI {
        export function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndCreateHtmlForWorkbenchs($("#container_workspace")).done(function (data) {
                }).fail(function () {
                });
            });
        }

        export function loadExtents(workspaceId: string) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForExtents($("#container_extents"), workspaceId).done(function (data) {
                }).fail(function () {
                });
            });
        }

        export function loadExtent(workspaceId: string, extentUrl: string) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItems($("#container_item"), workspaceId, extentUrl);
            });
        }
    }    
};
