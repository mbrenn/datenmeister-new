import * as Mof from "../Mof.js";
import {TableState} from "../forms/TableState.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

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
        const viewNodes = tableState.getViewNodes();

        // 1. Filter by FreeText (RowFilterByFreeTextAnywhere)
        const freeTextNodes = viewNodes.filter(
            n => n.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri
        );
        for (const freeTextNode of freeTextNodes) {
            const freeText = freeTextNode.get(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, Mof.ObjectType.String);
            const propertyName = freeTextNode.get(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.propertyName, Mof.ObjectType.String);
            
            if (freeText !== undefined && freeText !== null && freeText.trim() !== "") {
                const lowerText = freeText.toLowerCase();
                result = result.filter(element => this.matchesFreeText(element, lowerText, propertyName));
            }
        }

        // 2. Filter by Property Value (RowFilterByPropertyValueNode)
        const propFilterNodes = viewNodes.filter(
            n => n.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri
        );
        for (const propNode of propFilterNodes) {
            const property = propNode.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, Mof.ObjectType.String);
            const value = propNode.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, Mof.ObjectType.String);
            const comparisonMode = propNode.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.comparisonMode);

            if (property) {
                result = result.filter(element => {
                    const elemValue = element.get(property, Mof.ObjectType.String);
                    return this.matchesPropertyValue(elemValue, value, comparisonMode);
                });
            }
        }

        // 3. Filter by Metaclass (RowFilterByMetaclassNode)
        const metaClassFilterNodes = viewNodes.filter(
            n => n.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterByMetaclassNode_Uri
        );
        for (const metaNode of metaClassFilterNodes) {
            const metaClass = metaNode.get(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.metaClass, Mof.ObjectType.Object);
            const targetMetaUri = metaClass?.uri ?? (metaClass?.get ? metaClass.get("uri", Mof.ObjectType.String) : undefined);
            if (targetMetaUri) {
                result = result.filter(element => element.metaClass?.uri === targetMetaUri);
            }
        }

        // 4. Order / Sort (RowOrderByNode)
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

        // 5. Limit and Offset / Pagination (RowFilterOnPositionNode)
        const positionNode = viewNodes.find(
            n => n.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri
        );
        if (positionNode) {
            const position = positionNode.get(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.position, Mof.ObjectType.Number) ?? 0;
            const amount = positionNode.get(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, Mof.ObjectType.Number);

            const start = position > 0 ? position : 0;
            if (amount !== undefined && amount !== null && amount >= 0) {
                result = result.slice(start, start + amount);
            } else if (start > 0) {
                result = result.slice(start);
            }
        }

        return result;
    }

    private matchesFreeText(element: Mof.DmObject, freeTextLower: string, propertyName?: string): boolean {
        if (propertyName) {
            const val = element.get(propertyName);
            if (val !== undefined && val !== null) {
                return val.toString().toLowerCase().includes(freeTextLower);
            }
            return false;
        }

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

    private matchesPropertyValue(elementValue: string | undefined, filterValue: string, comparisonMode: any): boolean {
        if (elementValue === undefined || elementValue === null) {
            return false;
        }

        switch (comparisonMode) {
            case _DatenMeister._DataViews.___ComparisonMode.NotEqual:
            case _DatenMeister._DataViews._ComparisonMode.NotEqual:
                return elementValue !== filterValue;
            case _DatenMeister._DataViews.___ComparisonMode.Contains:
            case _DatenMeister._DataViews._ComparisonMode.Contains:
                return elementValue.includes(filterValue);
            case _DatenMeister._DataViews.___ComparisonMode.DoesNotContain:
            case _DatenMeister._DataViews._ComparisonMode.DoesNotContain:
                return !elementValue.includes(filterValue);
            case _DatenMeister._DataViews.___ComparisonMode.GreaterThan:
            case _DatenMeister._DataViews._ComparisonMode.GreaterThan:
                return elementValue > filterValue;
            case _DatenMeister._DataViews.___ComparisonMode.GreaterOrEqualThan:
            case _DatenMeister._DataViews._ComparisonMode.GreaterOrEqualThan:
                return elementValue >= filterValue;
            case _DatenMeister._DataViews.___ComparisonMode.LighterThan:
            case _DatenMeister._DataViews._ComparisonMode.LighterThan:
                return elementValue < filterValue;
            case _DatenMeister._DataViews.___ComparisonMode.LighterOrEqualThan:
            case _DatenMeister._DataViews._ComparisonMode.LighterOrEqualThan:
                return elementValue <= filterValue;
            case _DatenMeister._DataViews.___ComparisonMode.RegexMatch:
            case _DatenMeister._DataViews._ComparisonMode.RegexMatch:
                try {
                    return new RegExp(filterValue).test(elementValue);
                } catch {
                    return false;
                }
            case _DatenMeister._DataViews.___ComparisonMode.RegexNoMatch:
            case _DatenMeister._DataViews._ComparisonMode.RegexNoMatch:
                try {
                    return !new RegExp(filterValue).test(elementValue);
                } catch {
                    return true;
                }
            case _DatenMeister._DataViews.___ComparisonMode.Equal:
            case _DatenMeister._DataViews._ComparisonMode.Equal:
            default:
                return elementValue === filterValue;
        }
    }
}