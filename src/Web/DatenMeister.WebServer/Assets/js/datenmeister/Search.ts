import {findBySearchString} from "./client/Elements.js";
import {navigateToExtentItems, navigateToItemByUrl} from "./Navigator.js";

export function executeSearchByText(searchText: string) {
    findBySearchString(searchText).then(result => {
        switch (result.resultType) {
            case 'reference':
                navigateToItemByUrl(
                    result.reference.workspace,
                    result.reference.uri
                );
                break;
            case 'referenceExtent':
                if (result.reference.extentUri === undefined) {
                    throw new Error("Extent URI is undefined");
                }

                navigateToExtentItems(
                    result.reference.workspace,
                    result.reference.extentUri
                );
                break;
            case 'none':
                alert('Unfortunately, nothing was found.');
                break;
            default:
                alert('Unknown result type: ' + result.resultType);
        }
    });
}