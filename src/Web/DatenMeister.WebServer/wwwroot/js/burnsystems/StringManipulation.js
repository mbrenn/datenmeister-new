define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.truncateText = void 0;
    function truncateText(value, parameter) {
        let added = false;
        if ((parameter === null || parameter === void 0 ? void 0 : parameter.maxLength) !== undefined) {
            if (parameter.maxLength > 0 && value.length > parameter.maxLength) {
                value = value.substring(0, parameter.maxLength);
                added = true;
                if ((parameter === null || parameter === void 0 ? void 0 : parameter.useWordBoundary) === true) {
                    value = value.slice(0, value.lastIndexOf(" "));
                }
            }
        }
        if (parameter.maxLines !== undefined && parameter.maxLines > 0) {
            let lines = value.split('\n');
            if (lines.length > parameter.maxLines) {
                value = lines.splice(0, parameter.maxLines).join('\n').trim();
                value += "\n";
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