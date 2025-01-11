import * as Mof from "../Mof.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";

export class QueryBuilder {

    queryStatement: Mof.DmObject;

    constructor() {
        this.queryStatement = new Mof.DmObject(_DatenMeister._DataViews.__QueryStatement_Uri);
    }

    addNode(node: Mof.DmObject) {
        this.queryStatement.appendToArray(_DatenMeister._DataViews._QueryStatement.nodes, node);
    }

    setResultNode(node: Mof.DmObject) {
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, node);
    }

    getResultNode(): Mof.DmObject {
        return this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
    }
}

export function filterByMetaClass(builder: QueryBuilder, metaClass: Mof.DmObject, includeInherits: boolean) {
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.metaClass, metaClass);
    viewNode.set(_DatenMeister._DataViews._FilterByMetaclassNode.includeInherits, includeInherits);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function flatten(builder: QueryBuilder): Mof.DmObject {
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FlattenNode.input, builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function getElementsOfExtent(builder: QueryBuilder, workspaceId: string, extentUrl: string) {
    var viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.extentUri, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

