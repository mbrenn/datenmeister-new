import * as Mof from "../Mof.js";
import {TableState} from "../forms/TableState.js";

/**
 * Performs all necessary filtering and sorting upon the QueryRequest on a client-supplied list of MofObjects
 * It is used to filter just in JavaScript without interacting with the server
 */
export class StaticObjectQuery {
    
    /**
     * Stores the objects
     * @private
     */
    private objects: Mof.DmObject[];
    
    constructor(objects: Mof.DmObject[]) {
        this.objects = objects;
    }

    /**
     * Gets the filtered object upon the query
     * @param query Query of type DataView.QueryStatement
     */
    getFilteredObject(query: Mof.DmObject): Mof.DmObject[] {
        if (!this.objects) {
            return [];
        }

        if (!query) {
            return [...this.objects];
        }

        const tableState = new TableState();
        tableState.queryStatement = query;
        
        let result = [...this.objects];

        // 1. Filter by FreeText
        const freeText = tableState.getFreeTextFilter();
        if (freeText !== undefined && freeText !== null && freeText.trim() !== "") {
            const lowerText = freeText.toLowerCase();
            result = result.filter(element => this.matchesFreeText(element, lowerText));
        }

        // 2. Filter by Property Value
        const filterByProperties = tableState.getFilterByProperties();
        for (const property in filterByProperties) {
            if (Object.prototype.hasOwnProperty.call(filterByProperties, property)) {
                const value = filterByProperties[property];
                result = result.filter(element => {
                    const elemValue = element.get(property, Mof.ObjectType.String);
                    return elemValue === value;
                });
            }
        }

        // 3. Order / Sort
        const orderBy = tableState.getOrderBy();
        if (orderBy && orderBy.property) {
            const property = orderBy.property;
            const descending = orderBy.descending;

            result.sort((a, b) => {
                const valA = a.get(property);
                const valB = b.get(property);

                let cmp = 0;
                if (valA === undefined || valA === null) {
                    if (valB === undefined || valB === null) {
                        cmp = 0;
                    } else {
                        cmp = -1;
                    }
                } else if (valB === undefined || valB === null) {
                    cmp = 1;
                } else if (typeof valA === "number" && typeof valB === "number") {
                    cmp = valA - valB;
                } else if (typeof valA === "boolean" && typeof valB === "boolean") {
                    cmp = valA === valB ? 0 : valA ? 1 : -1;
                } else {
                    const strA = valA.toString();
                    const strB = valB.toString();
                    cmp = strA.localeCompare(strB);
                }

                return descending ? -cmp : cmp;
            });
        }

        // 4. Limit
        const limit = tableState.getLimit();
        if (limit !== undefined && limit !== null && limit >= 0) {
            result = result.slice(0, limit);
        }

        return result;
    }

    private matchesFreeText(element: Mof.DmObject, freeTextLower: string): boolean {
        const propValues = element.getPropertyValues();
        for (const key in propValues) {
            if (Object.prototype.hasOwnProperty.call(propValues, key)) {
                const val = propValues[key];
                if (val !== undefined && val !== null) {
                    if (val.toString().toLowerCase().includes(freeTextLower)) {
                        return true;
                    }
                }
            }
        }

        if (element.id && element.id.toLowerCase().includes(freeTextLower)) {
            return true;
        }
        if (element.uri && element.uri.toLowerCase().includes(freeTextLower)) {
            return true;
        }

        return false;
    }
}