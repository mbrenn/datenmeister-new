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
}

export function truncateText(value:string, parameter?: ITruncateParameters) {

    let added = false;
    if (parameter?.maxLength !== undefined) {
        if (parameter.maxLength > 0 && value.length) {
            value = value.substring(0, parameter.maxLength);
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