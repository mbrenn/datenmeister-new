import {findBySearchString} from "./Client.Elements";
import {navigateToExtent, navigateToItemByUrl} from "./Navigator";

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