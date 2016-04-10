define(["require", "exports"], function (require, exports) {
    "use strict";
    // Helper function out of http://stackoverflow.com/questions/901115/how-can-i-get-query-string-values-in-javascript
    function getParameterByNameFromHash(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&#]" + name + "=([^&#]*)"), results = regex.exec(location.hash);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    exports.getParameterByNameFromHash = getParameterByNameFromHash;
});
//# sourceMappingURL=datenmeister-helper.js.map