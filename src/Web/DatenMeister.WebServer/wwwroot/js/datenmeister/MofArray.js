define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.moveItemInArrayDownByUri = exports.moveItemInArrayUpByUri = void 0;
    /**
     * Takes the elements and moves the element with the given uri up by one position
     * The array itself is modified
     * @param elements elements containing the item
     * @param workspace The item's workspace which shall be moved by one position
     * @param itemUri the item which shall be moved up by one position
     */
    function moveItemInArrayUpByUri(elements, workspace, itemUri) {
        for (let i = 0; i < elements.length; i++) {
            const item = elements[i];
            if (item.workspace === workspace && item.uri === itemUri) {
                // Skip, if it is the first one
                if (i === 0)
                    return;
                elements.splice(i, 1);
                elements.splice(i - 1, 0, item);
                return;
            }
        }
    }
    exports.moveItemInArrayUpByUri = moveItemInArrayUpByUri;
    /**
     * Takes the elements and moves the element with the given uri down by one position
     * The array itself is modified
     * @param elements elements containing the item
     * @param workspace The item's workspace which shall be moved by one position
     * @param itemUri the item which shall be moved down by one position
     */
    function moveItemInArrayDownByUri(elements, workspace, itemUri) {
        // Go through the items, but skip the last one, because that one cannot be moved down anymore. 
        for (let i = 0; i < elements.length - 1; i++) {
            const item = elements[i];
            if (item.workspace === workspace && item.uri === itemUri) {
                elements.splice(i, 1);
                elements.splice(i + 1, 0, item);
                return;
            }
        }
    }
    exports.moveItemInArrayDownByUri = moveItemInArrayDownByUri;
});
//# sourceMappingURL=MofArray.js.map