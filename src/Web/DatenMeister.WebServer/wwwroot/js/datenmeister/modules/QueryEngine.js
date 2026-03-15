import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
export class QueryBuilder {
    constructor() {
        this.queryStatement = new Mof.DmObject(_DatenMeister._DataViews.__QueryStatement_Uri);
    }
    addNode(node) {
        this.queryStatement.appendToArray(_DatenMeister._DataViews._QueryStatement.nodes, node);
    }
    setResultNode(node) {
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.DmObject.createAsReferenceFromLocalId(node));
    }
    getResultNode() {
        return this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
    }
}
export function createForReferenceExistingNode(workspaceId, nodeUri) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Node.__ReferenceViewNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.itemUri, nodeUri);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode._name_, "Reference to " + nodeUri);
    return viewNode;
}
export function referenceExistingNode(builder, workspaceId, nodeUri) {
    const viewNode = createForReferenceExistingNode(workspaceId, nodeUri);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForFilterByMetaClass(input, metaClass, includeInherits) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.metaClass, metaClass);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode._name_, "Filter by metaclass " + metaClass.uri);
    if (includeInherits !== undefined) {
        viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.includeInherits, includeInherits);
    }
    return viewNode;
}
export function filterByMetaClass(builder, metaClass, includeInherits) {
    const viewNode = createForFilterByMetaClass(builder.getResultNode(), metaClass, includeInherits);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForOrderByProperty(input, property, descending) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.propertyName, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.orderDescending, descending);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode._name_, "Order by " + property + (descending ? " descending" : " ascending"));
    return viewNode;
}
export function orderByProperty(builder, property, descending) {
    const viewNode = createForOrderByProperty(builder.getResultNode(), property, descending);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForLimit(input, limit) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, limit);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode._name_, "Limit to " + limit + " elements");
    return viewNode;
}
export function limit(builder, limitValue) {
    const viewNode = createForLimit(builder.getResultNode(), limitValue);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForFilterByFreetext(input, freeText) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, freeText);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere._name_, "Filter by free text " + freeText);
    return viewNode;
}
export function filterByFreetext(builder, freeText) {
    const viewNode = createForFilterByFreetext(builder.getResultNode(), freeText);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForFilterByProperty(input, property, value, comparisonMode) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, comparisonMode ?? _DatenMeister._DataViews._ComparisonMode.Equal);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode._name_, "Filter by property " + property + " with value " + value + " (" + comparisonMode + ")");
    return viewNode;
}
export function filterByProperty(builder, property, value, comparisonMode) {
    const viewNode = createForFilterByProperty(builder.getResultNode(), property, value, comparisonMode);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForFlatten(input) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFlattenNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Row._RowFlattenNode._name_, "Flatten");
    return viewNode;
}
export function flatten(builder) {
    const viewNode = createForFlatten(builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForAddDynamicSource(name) {
    const dynamicSource = new Mof.DmObject(_DatenMeister._DataViews._Source.__DynamicSourceNode_Uri);
    dynamicSource.set(_DatenMeister._DataViews._Source._DynamicSourceNode.nodeName, name);
    dynamicSource.set(_DatenMeister._DataViews._Source._DynamicSourceNode._name_, "Dynamic source " + name);
    return dynamicSource;
}
export function addDynamicSource(builder, name) {
    const dynamicSource = createForAddDynamicSource(name);
    builder.addNode(dynamicSource);
    builder.setResultNode(dynamicSource);
    return dynamicSource;
}
export function createForGetElementsOfExtent(workspaceId, extentUrl) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.extentUri, extentUrl);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode._name_, "Select by extent " + extentUrl);
    return viewNode;
}
export function getElementsOfExtent(builder, workspaceId, extentUrl) {
    const viewNode = createForGetElementsOfExtent(workspaceId, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForGetElementsByPath(workspaceId, path) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByPathNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.path, path);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode._name_, "Select by path " + path);
    return viewNode;
}
export function getElementsByPath(builder, workspaceId, path) {
    const viewNode = createForGetElementsByPath(workspaceId, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForColumnFilterIncludeOnly(input, columnNamesComma) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterIncludeOnlyNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.columnNamesComma, columnNamesComma);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode._name_, "Include-Only columns " + columnNamesComma);
    return viewNode;
}
export function columnFilterIncludeOnly(builder, columnNamesComma) {
    const viewNode = createForColumnFilterIncludeOnly(builder.getResultNode(), columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function createForColumnFilterExclude(input, columnNamesComma) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterExcludeNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.input, input);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.columnNamesComma, columnNamesComma);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode._name_, "Exclude columns " + columnNamesComma);
    return viewNode;
}
export function columnFilterExclude(builder, columnNamesComma) {
    const viewNode = createForColumnFilterExclude(builder.getResultNode(), columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
//# sourceMappingURL=QueryEngine.js.map