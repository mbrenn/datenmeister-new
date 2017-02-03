define(["require", "exports"], function (require, exports) {
    "use strict";
    // Helper function out of http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
    function getParameterByNameFromHash(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"), results = regex.exec(location.hash);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    exports.getParameterByNameFromHash = getParameterByNameFromHash;
    exports.simpleEvent = function (context) {
        if (context === void 0) { context = null; }
        var cbs = [];
        return {
            addListener: function (cb) {
                cbs.push(cb);
            },
            removeListener: function (cb) {
                var i = cbs.indexOf(cb);
                cbs.splice(i, Math.max(i, 0));
            },
            trigger: function () {
                var args = [];
                for (var _i = 0; _i < arguments.length; _i++) {
                    args[_i] = arguments[_i];
                }
                cbs.forEach(function (cb) { return cb.apply(context, args); });
            }
        };
    };
    var SimpleEventClass = (function () {
        function SimpleEventClass(context) {
            if (context === void 0) { context = null; }
            this.context = context;
            this.cbs = new Array();
        }
        SimpleEventClass.prototype.addListener = function (cb) {
            this.cbs.push(cb);
        };
        SimpleEventClass.prototype.removeListener = function (cb) {
            var i = this.cbs.indexOf(cb);
            this.cbs.splice(i, Math.max(i, 0));
        };
        SimpleEventClass.prototype.trigger = function () {
            var _this = this;
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            this.cbs.forEach(function (cb) { return cb.apply(_this.context, args); });
        };
        return SimpleEventClass;
    }());
    exports.SimpleEventClass = SimpleEventClass;
});
//# sourceMappingURL=datenmeister-helper.js.map