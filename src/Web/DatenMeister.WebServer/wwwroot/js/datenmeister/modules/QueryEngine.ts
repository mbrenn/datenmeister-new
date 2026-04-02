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
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode,
            Mof.DmObject.createAsReferenceFromLocalId(node));
    }

    getResultNode(): Mof.DmObject {
        return this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
    }
}

export function createForReferenceExistingNode(workspaceId: string, nodeUri: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Node.__ReferenceViewNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode.itemUri, nodeUri);
    viewNode.set(_DatenMeister._DataViews._Node._ReferenceViewNode._name_, "Reference to " + nodeUri);
    return viewNode;
}

export function referenceExistingNode(builder: QueryBuilder, workspaceId: string, nodeUri: string) {
    const viewNode = createForReferenceExistingNode(workspaceId, nodeUri);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForFilterByMetaClass(input: Mof.DmObject | undefined, metaClass: Mof.DmObject, includeInherits?: boolean) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByMetaclassNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.metaClass, metaClass);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode._name_, "Filter by metaclass " + metaClass.uri);
    if (includeInherits !== undefined) {
        viewNode.set(_DatenMeister._DataViews._Row._RowFilterByMetaclassNode.includeInherits, includeInherits)
    }

    return viewNode;
}

export function filterByMetaClass(builder: QueryBuilder, metaClass: Mof.DmObject, includeInherits?: boolean) {
    const viewNode = createForFilterByMetaClass(builder.getResultNode(), metaClass, includeInherits);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForOrderByProperty(input: Mof.DmObject | undefined, property: string, descending: boolean) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.propertyName, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode.orderDescending, descending);
    viewNode.set(_DatenMeister._DataViews._Row._RowOrderByNode._name_, "Order by " + property + (descending ? " descending" : " ascending"));
    return viewNode;
}

export function orderByProperty(builder: QueryBuilder, property: string, descending: boolean) {
    const viewNode = createForOrderByProperty(builder.getResultNode(), property, descending);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForLimit(input: Mof.DmObject | undefined, limit: number) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, limit);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode._name_, "Limit to " + limit + " elements");
    return viewNode;
}

export function limit(builder: QueryBuilder, limitValue: number) {
    const viewNode = createForLimit(builder.getResultNode(), limitValue);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForFilterByFreetext(input: Mof.DmObject | undefined, freeText: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, freeText);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere._name_, "Filter by free text " + freeText);
    return viewNode;
}

export function filterByFreetext(builder: QueryBuilder, freeText: string) {
    const viewNode = createForFilterByFreetext(builder.getResultNode(), freeText);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForFilterByProperty(input: Mof.DmObject | undefined,
                                   property: string,
                                   value: string,
                                   comparisonMode?: _DatenMeister._DataViews.___ComparisonMode) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, property);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, value);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.comparisonMode,
        comparisonMode ?? _DatenMeister._DataViews._ComparisonMode.Equal);
    viewNode.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode._name_, "Filter by property " + property + " with value '" + value + "' (" + comparisonMode + ")");
    return viewNode;
}

export function filterByProperty(builder: QueryBuilder, 
                                 property: string,
                                 value: string,
                                 comparisonMode?: _DatenMeister._DataViews.___ComparisonMode) {
    const viewNode = createForFilterByProperty(builder.getResultNode(), property, value, comparisonMode);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForFlatten(input: Mof.DmObject | undefined): Mof.DmObject {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Row.__RowFlattenNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Row._RowFlattenNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Row._RowFlattenNode._name_, "Flatten");
    return viewNode;
}

export function flatten(builder: QueryBuilder): Mof.DmObject {
    const viewNode = createForFlatten(builder.getResultNode());
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForAddDynamicSource(name: string) {
    const dynamicSource = new Mof.DmObject(_DatenMeister._DataViews._Source.__DynamicSourceNode_Uri);
    dynamicSource.set(_DatenMeister._DataViews._Source._DynamicSourceNode.nodeName, name);
    dynamicSource.set(_DatenMeister._DataViews._Source._DynamicSourceNode._name_, "Dynamic source " + name);
    return dynamicSource;
}

export function addDynamicSource(builder: QueryBuilder, name: string)
{
    const dynamicSource = createForAddDynamicSource(name);
    builder.addNode(dynamicSource);
    builder.setResultNode(dynamicSource);
    return dynamicSource;
}

export function createForGetElementsOfExtent(workspaceId: string, extentUrl: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByExtentNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode.extentUri, extentUrl);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByExtentNode._name_, "Select by extent " + extentUrl);
    return viewNode;
}

export function getElementsOfExtent(builder: QueryBuilder, workspaceId: string, extentUrl: string) {
    const viewNode = createForGetElementsOfExtent(workspaceId, extentUrl);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForGetElementsByPath(workspaceId: string, path: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Source.__SelectByPathNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.workspaceId, workspaceId);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode.path, path);
    viewNode.set(_DatenMeister._DataViews._Source._SelectByPathNode._name_, "Select by path " + path);
    return viewNode;
}

export function getElementsByPath(builder: QueryBuilder, workspaceId: string, path: string) {
    const viewNode = createForGetElementsByPath(workspaceId, path);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForColumnFilterIncludeOnly(input: Mof.DmObject | undefined, columnNamesComma: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterIncludeOnlyNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode.columnNamesComma, columnNamesComma);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterIncludeOnlyNode._name_, "Include-Only columns " + columnNamesComma);
    return viewNode;
}

export function columnFilterIncludeOnly(builder: QueryBuilder, columnNamesComma: string) {
    const viewNode = createForColumnFilterIncludeOnly(builder.getResultNode(), columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

export function createForColumnFilterExclude(input: Mof.DmObject | undefined, columnNamesComma: string) {
    const viewNode = new Mof.DmObject(_DatenMeister._DataViews._Column.__ColumnFilterExcludeNode_Uri);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.input, Mof.DmObject.createAsReferenceFromLocalId(input));
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode.columnNamesComma, columnNamesComma);
    viewNode.set(_DatenMeister._DataViews._Column._ColumnFilterExcludeNode._name_, "Exclude columns " + columnNamesComma);
    return viewNode;
}

export function columnFilterExclude(builder: QueryBuilder, columnNamesComma: string) {
    const viewNode = createForColumnFilterExclude(builder.getResultNode(), columnNamesComma);
    builder.addNode(viewNode);
    builder.setResultNode(viewNode);
    return viewNode;
}

