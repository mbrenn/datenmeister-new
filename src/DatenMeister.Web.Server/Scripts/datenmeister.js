/// <reference path="typings/jquery/jquery.d.ts" />
var DatenMeister;
(function (DatenMeister) {
    ;
    var WorkbenchLogic = (function () {
        function WorkbenchLogic() {
        }
        WorkbenchLogic.prototype.loadWorkbenchs = function () {
            var callback = $.Deferred();
            $.ajax("/api/datenmeister/workspace/all").
                done(function (data) {
                callback.resolve(null);
            })
                .fail(function (data) {
                callback.reject(null);
            });
            return callback;
        };
        return WorkbenchLogic;
    })();
    DatenMeister.WorkbenchLogic = WorkbenchLogic;
    ;
})(DatenMeister || (DatenMeister = {}));
;
$(document).ready(function () {
    var workbenchLogic = new DatenMeister.WorkbenchLogic();
    workbenchLogic.loadWorkbenchs().done(function (data) {
        alert('We succeeded');
    }).fail(function () {
        alert('We failed');
    });
});
//# sourceMappingURL=datenmeister.js.map