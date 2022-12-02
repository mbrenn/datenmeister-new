export interface ITruncateParameters
{
    /**
     * Maximum length of the string which will be returned
     */
    maxLength?: number;

    /**
     * Truncate length
     */
    truncateEllipsis?: string;

    /**
     * Truncates at word boundary
     */
    useWordBoundary?: boolean;

    /**
     * Defines the number of maximum lines
     */
    maxLines?: number;
}

export function truncateText(value:string, parameter?: ITruncateParameters) {

    let added = false;
    if (parameter?.maxLength !== undefined) {
        if (parameter.maxLength > 0 && value.length > parameter.maxLength) {
            value = value.substring(0, parameter.maxLength);
            added = true;

            if (parameter?.useWordBoundary === true) {
                value = value.slice(0, value.lastIndexOf(" "))
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
        const ellipses = parameter?.truncateEllipsis === undefined ?
            " …" : parameter.truncateEllipsis;
        value += ellipses;
    }

    return value;
}