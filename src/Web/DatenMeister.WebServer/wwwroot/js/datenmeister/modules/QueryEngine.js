import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import { DmObject } from "../Mof.js";
export class QueryBuilder {
    constructor() {
        this.queryStatement = new Mof.DmObject(_DatenMeister._DataViews.__QueryStatement_Uri);
    }
    addNode(node) {
        this.queryStatement.appendToArray(_DatenMeister._DataViews._QueryStatement.nodes, node);
    }
    setResultNode(node) {
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, DmObject.createAsReferenceFromLocalId(node));
    }
    getResultNode() {
        return this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
    }
}
export function referenceExistingNode(builder, workspaceId, nodeUri) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Node.__ReferenceViewNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.itemUri, nodeUri);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function filterByMetaClass(builder, metaClass, includeInherits) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.metaClass, metaClass);
    if (includeInherits !== undefined) {
        viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.includeInherits, includeInherits);
    }
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function orderByProperty(builder, property, descending) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.propertyName, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.orderDescending, descending);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function limit(builder, limit) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, limit);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function filterByFreetext(builder, freeText) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, freeText);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function filterByProperty(builder, property, value, comparisonMode) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.comparisonMode, comparisonMode ?? _DatenMeister._DataViews._ComparisonMode.Equal);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function flatten(builder) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFlattenNode.input, builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function addDynamicSource(builder, name) {
    const dynamicSource = new Mof.DmObject(_DatenMeister._DataViews._Source.__DynamicSourceNode_Uri);
    dynamicSource.set(_DatenMeister._DataViews._Source._DynamicSourceNode.nodeName, name);
    builder.addNode(dynamicSource);
    builder.setResultNode(dynamicSource);
    return dynamicSource;
}
export function getElementsOfExtent(builder, workspaceId, extentUrl) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.extentUri, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function getElementsByPath(builder, workspaceId, path) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByPathNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.path, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function columnFilterIncludeOnly(builder, columnNamesComma) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterIncludeOnlyNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.columnNamesComma, columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function columnFilterExclude(builder, columnNamesComma) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterExcludeNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.columnNamesComma, columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
//# sourceMappingURL=QueryEngine.js.map