define(["require", "exports", "./Client.Elements", "./Navigator"], function (require, exports, Client_Elements_1, Navigator_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.executeSearchByText = void 0;
    function executeSearchByText(searchText) {
        (0, Client_Elements_1.findBySearchString)(searchText).done(result => {
            switch (result.resultType) {
                case 'reference':
                    (0, Navigator_1.navigateToItemByUrl)(result.reference.workspace, result.reference.uri);
                    break;
                case 'referenceExtent':
                    (0, Navigator_1.navigateToExtent)(result.reference.workspace, result.reference.extentUri);
                    break;
                case 'none':
                    alert('Unfortunately, nothing was found.');
                    break;
                default:
                    alert('Unknown result type: ' + result.resultType);
            }
        });
    }
    exports.executeSearchByText = executeSearchByText;
});
//# sourceMappingURL=Search.js.map