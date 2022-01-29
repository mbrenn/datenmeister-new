import {findBySearchString} from "./Client.Elements";
import {navigateToExtent, navigateToItem} from "./Navigator";

export function executeSearchByText(searchText: string) {
    findBySearchString(searchText).done(result => {
        switch (result.resultType) {
            case 'reference':
                navigateToItem(
                    result.reference.workspace,
                    result.reference.extentUri,
                    result.reference.id
                );
                break;
            case 'referenceExtent':
                navigateToExtent(
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