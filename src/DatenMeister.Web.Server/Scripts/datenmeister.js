/// <reference path="typings/jquery/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    ;
    var WorkspaceLogic = (function () {
        function WorkspaceLogic() {
        }
        WorkspaceLogic.prototype.loadAndCreateHtmlForWorkbenchs = function (container) {
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
        };
        WorkspaceLogic.prototype.createHtmlForWorkbenchs = function (container, data) {
            container.empty();
            var compiled = _.template($("#template_workspace").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = "/Home/workspace?ws=" + localEntry.id;
                    };
                }(entry)));
                container.append(dom);
            }
        };
        return WorkspaceLogic;
    })();
    DatenMeister.WorkspaceLogic = WorkspaceLogic;
    ;
    var ExtentLogic = (function () {
        function ExtentLogic() {
        }
        ExtentLogic.prototype.loadAndCreateHtmlForExtents = function (container, ws) {
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
        };
        ExtentLogic.prototype.createHtmlForExtent = function (container, ws, data) {
            container.empty();
            var compiled = _.template($("#template_extent").html());
            for (var n in data) {
                var entry = data[n];
                var line = compiled(entry);
                var dom = $(line);
                $(".data", dom).click((function (localEntry) {
                    return function () {
                        location.href = "/Home/extent?ws=" + ws + "&extent=" + localEntry.uri;
                    };
                }(entry)));
                container.append(dom);
            }
        };
        ExtentLogic.prototype.loadAndCreateHtmlForItems = function (container, ws, extentUrl) {
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
        };
        ExtentLogic.prototype.createHtmlForItems = function (container, ws, extentUrl, data) {
            container.empty();
        };
        return ExtentLogic;
    })();
    DatenMeister.ExtentLogic = ExtentLogic;
    var GUI;
    (function (GUI) {
        function loadWorkspaces() {
            $(document).ready(function () {
                var workbenchLogic = new DatenMeister.WorkspaceLogic();
                workbenchLogic.loadAndCreateHtmlForWorkbenchs($("#container_workspace")).done(function (data) {
                }).fail(function () {
                });
            });
        }
        GUI.loadWorkspaces = loadWorkspaces;
        function loadExtents(workspaceId) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForExtents($("#container_extents"), workspaceId).done(function (data) {
                }).fail(function () {
                });
            });
        }
        GUI.loadExtents = loadExtents;
        function loadExtent(workspaceId, extentUrl) {
            $(document).ready(function () {
                var extentLogic = new DatenMeister.ExtentLogic();
                extentLogic.loadAndCreateHtmlForItems($("#container_item"), workspaceId, extentUrl);
            });
        }
        GUI.loadExtent = loadExtent;
    })(GUI = DatenMeister.GUI || (DatenMeister.GUI = {}));
})(DatenMeister || (DatenMeister = {}));
;
//# sourceMappingURL=datenmeister.js.map