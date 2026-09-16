export var FormType;
(function (FormType) {
    FormType["Object"] = "object";
    FormType["Collection"] = "collection";
})(FormType || (FormType = {}));
/**
 * Defines the filter parameters when a query shall be executed upon a set of items to allow
 * filtering, sorting or other data transformations directly on server-side
 */
export class QueryFilterParameter {
    orderBy; // Property to which the ordering shall be done
    orderByDescending; // Flag, whether ordering shall be done by descending
    filterByProperties; // Property filters. Key is Propertyname, Value is textfilter
    filterByFreetext; // Additional freetext
    /**
     * Defines the url to be used for the query. If not defined, the query will not be used
     */
    queryUrl;
    /**
     * Defines the workspace to be used for the query. If not defined, the default workspace is used.
     */
    queryWorkspace;
    /**
     * Defines the columns which shall be included in the output.
     * The columns are separated by comma
     */
    columnsIncludeOnly;
    /**
     * Defines the columns which shall be excluded from the output.
     * The columns are separated by comma
     */
    columnsExclude;
}
//# sourceMappingURL=Interfaces.js.map