import * as Mof from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
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
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.metaClass, metaClass);
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.includeInherits, includeInherits);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function flatten(builder) {
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FlattenNode.input, builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
export function getElementsOfExtent(builder, workspaceId, extentUrl) {
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.extentUri, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}
//# sourceMappingURL=QueryEngine.js.map