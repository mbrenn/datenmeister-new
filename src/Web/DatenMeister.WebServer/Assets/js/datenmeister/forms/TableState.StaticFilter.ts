import * as Mof from "../Mof.js";
import * as TableForm from "./TableForm.js";

export function filterAndSortTableData(
    tableForm: TableForm.TableForm,
    elementsAsDmObject: Mof.DmObjectWithSync[]) {
    return async (query: Mof.DmObject) => {
        const tableState = tableForm.tableState;
        if (query && tableState) {
            tableState.queryStatement = query;
        }

        let result = [...elementsAsDmObject];

        if (!tableState) {
            return result;
        }

        // 1. Free-Text Search via tableState.getFreeTextFilter()
        const freeText = tableState.getFreeTextFilter();
        if (freeText && freeText.trim().length > 0) {
            const term = freeText.trim().toLowerCase()
            result = result.filter((item) => {
                const propertyValues = item.getPropertyValues();
                for (let prop in propertyValues) {
                    if (propertyValues[prop].toLowerCase().includes(term))
                        return true;
                }

                return false;
            });
        }

        // 2. Property Filtering via tableState.getFilterByProperties()
        const propertyFilters = tableState.getFilterByProperties();
        if (propertyFilters) {
            for (var prop in propertyFilters) {
                const filterVal = propertyFilters[prop];
                result = result.filter((item) => item.get(prop, Mof.ObjectType.String) === filterVal);
            }
        }

        // 3. Sorting via tableState.getOrderBy()
        const orderBy = tableState.getOrderBy();
        if (orderBy && orderBy.property) {
            const prop = orderBy.property;
            const isDesc = orderBy.descending;

            result.sort((a, b) => {
                let aValue = a.get(prop);
                let bValue = b.get(prop);

                if (aValue === undefined) {
                    return isDesc ? 1 : -1;
                }
                if (bValue === undefined) {
                    return isDesc ? -1 : 1;
                }
                if (typeof aValue === "number" && typeof bValue === "number") {
                    return aValue - bValue * (isDesc ? -1 : 1);
                }

                aValue = aValue.toString();
                bValue = bValue.toString();
                return aValue.localeCompare(bValue) * (isDesc ? -1 : 1);
            });
        }

        // 4. Limiting / Pagination via tableState.getLimit()
        let limit = tableState.getLimit();
        if (limit === undefined) {
            limit = 100;
            if (result.length > limit) {
                tableForm.setInfoText("Only " + limit + " items are shown");
            }
        }

        if (limit > 0) {
            result = result.slice(0, limit);
        }

        return result;
    };
}