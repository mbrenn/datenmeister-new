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
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, node);
    }
    getResultNode() {
        return this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
    }
}
export function filterByMetaClass(builder, metaClass, includeInherits) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__RowFilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.metaClass, metaClass);
    if (includeInherits !== undefined) {
        viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.includeInherits, includeInherits);
    }
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function orderByProperty(builder, property, descending) {
    // Not Implemented up to now
}
export function limit(builder, limit) {
    // not implemented up to now
}
export function filterByProperty(builder, property, value, comparisonMode) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.comparisonMode, comparisonMode ?? _DatenMeister._DataViews._ComparisonMode.Equal);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function flatten(builder) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FlattenNode.input, builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function getElementsOfExtent(builder, workspaceId, extentUrl) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.extentUri, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function getElementsByPath(builder, workspaceId, path) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByPathNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByPathNode.path, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
//# sourceMappingURL=QueryEngine.js.map