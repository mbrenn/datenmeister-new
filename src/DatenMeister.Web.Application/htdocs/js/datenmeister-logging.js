/// <reference path="typings/jquery/jquery.d.ts" />
/// <reference path="typings/jquery/underscore.d.ts" />
define(["require", "exports"], function (require, exports) {
    var Logging = (function () {
        function Logging() {
        }
        Logging.prototype.writeMessage = function (value) {
            var line = this.stringifyForDialog(value);
            alert("A message was given:\r\n" + line);
        };
        Logging.prototype.writeError = function (value) {
            var line = this.stringifyForDialog(value);
            alert("An error has occured:\r\n" + line);
        };
        Logging.prototype.writeWarning = function (value) {
            var line = this.stringifyForDialog(value);
            alert("A warning has occured:\r\n" + line);
        };
        Logging.prototype.stringifyForDialog = function (value) {
            return JSON.stringify(value, null, "    ");
        };
        return Logging;
    })();
    exports.Logging = Logging;
    exports.theLog = new Logging();
});
//# sourceMappingURL=datenmeister-logging.js.map