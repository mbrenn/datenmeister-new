import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

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

export function filterByMetaClass(builder: QueryBuilder, metaClass: Mof.DmObject, includeInherits?: boolean) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__RowFilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.metaClass, metaClass);
    if (includeInherits !== undefined) {
        viewNode.set(_DatenMeister._DataViews._RowFilterByMetaclassNode.includeInherits, includeInherits)
    }

    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function orderByProperty(builder: QueryBuilder, property: string, descending: boolean) {
    // Not Implemented up to now
}

export function limit(builder: QueryBuilder, limit: number) {
    // not implemented up to now
}

export function filterByProperty(builder: QueryBuilder, 
                                 property: string,
                                 value: string,
                                 comparisonMode?: _DatenMeister._DataViews.___ComparisonMode) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.input, builder.getResultNode());
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DatenMeister._DataViews._RowFilterByPropertyValueNode.comparisonMode,
        comparisonMode ?? _DatenMeister._DataViews._ComparisonMode.Equal);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    
    return viewNode;
}

export function flatten(builder: QueryBuilder): Mof.DmObject {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__FlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._FlattenNode.input, builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function getElementsOfExtent(builder: QueryBuilder, workspaceId: string, extentUrl: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByExtentNode.extentUri, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function getElementsByPath(builder: QueryBuilder, workspaceId: string, path: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews.__SelectByPathNode_Uri);
    viewNode.set(_DatenMeister._DataViews._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._SelectByPathNode.path, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

