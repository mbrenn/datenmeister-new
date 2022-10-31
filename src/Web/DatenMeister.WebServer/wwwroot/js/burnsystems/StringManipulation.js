define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.truncateText = void 0;
    function truncateText(value, parameter) {
        let added = false;
        if ((parameter === null || parameter === void 0 ? void 0 : parameter.maxLength) !== undefined) {
            if (parameter.maxLength > 0 && value.length) {
                value = value.substring(0, parameter.maxLength);
                added = true;
            }
        }
        if (added) {
            const ellipses = (parameter === null || parameter === void 0 ? void 0 : parameter.truncateEllipsis) === undefined ?
                " …" : parameter.truncateEllipsis;
            value += ellipses;
        }
        return value;
    }
    exports.truncateText = truncateText;
});
//# sourceMappingURL=StringManipulation.js.map