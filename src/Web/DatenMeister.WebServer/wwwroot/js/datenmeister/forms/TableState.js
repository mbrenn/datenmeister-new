import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as QueryEngine from "../modules/QueryEngine.js";
export class TableState {
    queryStatement = new Mof.DmObject(_DatenMeister._DataViews.__QueryStatement_Uri);
    overrideQueryWorkspace = undefined;
    overrideQueryItem = undefined;
    getOrderBy() {
        const node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
        if (node) {
            return {
                property: node.get(_DatenMeister._DataViews._Row._RowOrderByNode.propertyName, Mof.ObjectType.String),
                descending: node.get(_DatenMeister._DataViews._Row._RowOrderByNode.orderDescending, Mof.ObjectType.Boolean)
            };
        }
        return undefined;
    }
    setOrderBy(property, descending) {
        let node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
        if (!node) {
            node = QueryEngine.createForOrderByProperty(undefined, property, descending);
            this.addNode(node);
        }
        else {
            node.set(_DatenMeister._DataViews._Row._RowOrderByNode.name, "Order by '" + property + "' " + (descending ? "descending" : "ascending"));
            node.set(_DatenMeister._DataViews._Row._RowOrderByNode.propertyName, property);
            node.set(_DatenMeister._DataViews._Row._RowOrderByNode.orderDescending, descending);
        }
    }
    removeOrderBy() {
        this.removeNodeByMetaClass(_DatenMeister._DataViews._Row.__RowOrderByNode_Uri);
    }
    getFreeTextFilter() {
        const node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
        return node?.get(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, Mof.ObjectType.String);
    }
    setFreeTextFilter(text) {
        let node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
        if (!node) {
            node = QueryEngine.createForFilterByFreetext(undefined, text);
            this.addNode(node);
        }
        else {
            node.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.name, "Filter by free text '" + text + "'");
            node.set(_DatenMeister._DataViews._Row._RowFilterByFreeTextAnywhere.freeText, text);
        }
    }
    removeFreeTextFilter() {
        this.removeNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterByFreeTextAnywhere_Uri);
    }
    getFilterByProperty(property) {
        const node = this.findFilterByPropertyNode(property);
        return node?.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, Mof.ObjectType.String);
    }
    setFilterByProperty(property, value) {
        let node = this.findFilterByPropertyNode(property);
        const comparisonMode = _DatenMeister._DataViews.___ComparisonMode.Equal;
        if (!node) {
            node = QueryEngine.createForFilterByProperty(undefined, property, value, comparisonMode);
            this.addNode(node);
        }
        else {
            node.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.name, "Filter by property '" + property + "' " + comparisonMode + " '" + value + "'");
            node.set(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, value);
        }
    }
    removeFilterByProperty(property) {
        const node = this.findFilterByPropertyNode(property);
        if (node) {
            this.removeNode(node);
        }
    }
    getLimit() {
        const node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
        return node?.get(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, Mof.ObjectType.Number);
    }
    setLimit(limit) {
        let node = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
        if (node === undefined) {
            node = QueryEngine.createForLimit(undefined, limit);
            node.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.position, 0);
            this.addNode(node);
        }
        else {
            node.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.name, "Limit to " + limit + " items");
            node.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.amount, limit);
        }
    }
    removeLimit() {
        this.removeNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
    }
    getFilterByProperties() {
        const result = {};
        for (const node of this.getViewNodes()) {
            if (node.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri) {
                const prop = node.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, Mof.ObjectType.String);
                const val = node.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.value, Mof.ObjectType.String);
                if (prop) {
                    result[prop] = val;
                }
            }
        }
        return result;
    }
    /**
     * Gets the nodes of the query statement in the order from source to result
     */
    getViewNodes() {
        const result = [];
        let currentNode = this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
        while (currentNode) {
            result.push(currentNode);
            currentNode = currentNode.get("input", Mof.ObjectType.Object);
        }
        return result;
    }
    findNodeByMetaClass(metaClassUri) {
        return this.getViewNodes().find(n => n.metaClass?.uri === metaClassUri);
    }
    findFilterByPropertyNode(property) {
        return this.getViewNodes().find(n => n.metaClass?.uri === _DatenMeister._DataViews._Row.__RowFilterByPropertyValueNode_Uri &&
            n.get(_DatenMeister._DataViews._Row._RowFilterByPropertyValueNode.property, Mof.ObjectType.String) === property);
    }
    addNode(node) {
        // Find the limit node, because it must be the last node
        const limitNode = this.findNodeByMetaClass(_DatenMeister._DataViews._Row.__RowFilterOnPositionNode_Uri);
        if (limitNode && node !== limitNode) {
            // We have a limit node and the node to be added is not the limit node
            // So we insert the new node BEFORE the limit node
            const inputOfLimit = limitNode.get("input", Mof.ObjectType.Object);
            if (inputOfLimit) {
                node.set("input", inputOfLimit);
            }
            limitNode.set("input", node);
            this.queryStatement.appendToArray(_DatenMeister._DataViews._QueryStatement.nodes, node);
        }
        else {
            const resultNode = this.queryStatement.get(_DatenMeister._DataViews._QueryStatement.resultNode, Mof.ObjectType.Object);
            if (resultNode) {
                node.set("input", resultNode);
            }
            this.queryStatement.appendToArray(_DatenMeister._DataViews._QueryStatement.nodes, node);
            this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, node);
        }
    }
    removeNodeByMetaClass(metaClassUri) {
        const node = this.findNodeByMetaClass(metaClassUri);
        if (node) {
            this.removeNode(node);
        }
    }
    removeNode(node) {
        const nodes = this.queryStatement.getAsArray(_DatenMeister._DataViews._QueryStatement.nodes);
        const index = nodes.indexOf(node);
        if (index === -1)
            return;
        // Find the node that has the node to be removed as input
        const childNode = nodes.find(n => n.get("input", Mof.ObjectType.Object) === node);
        // Finds the input of the node to be removed
        const inputNode = node.get("input", Mof.ObjectType.Object);
        if (childNode) {
            if (inputNode) {
                // Both are existing, A -> B + B -> C ==> A -> C
                childNode.set("input", inputNode);
            }
            else {
                // The node to be removed does not have an input. So the node which is referencing the node
                // to be removed, will not have any input anymore
                childNode.unset("input");
            }
        }
        else {
            // There was no node which uses the node to be removed as input node
            // ==> If this node gets removed, its input will be the new result node
            if (inputNode) {
                this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.resultNode, inputNode);
            }
            else {
                this.queryStatement.unset(_DatenMeister._DataViews._QueryStatement.resultNode);
            }
        }
        // Remove the node from the list
        nodes.splice(index, 1);
        // Update the query statement with the new list of nodes
        this.queryStatement.set(_DatenMeister._DataViews._QueryStatement.nodes, nodes);
    }
    initialize(limit) {
        const builder = new QueryEngine.QueryBuilder();
        if (this.overrideQueryWorkspace !== undefined && this.overrideQueryItem !== undefined) {
            // In case we are just using query, we use that one as input. 
            QueryEngine.referenceExistingNode(builder, this.overrideQueryWorkspace, this.overrideQueryItem);
        }
        else {
            QueryEngine.addDynamicSource(builder, "input");
        }
        // Imposes only a limit in case it is not defined or positive
        // in case the given limit < 0, then no limit is applied
        if (limit === undefined) {
            QueryEngine.limit(builder, 101);
        }
        if (limit > 0) {
            QueryEngine.limit(builder, limit);
        }
        this.queryStatement = builder.queryStatement;
    }
}
//# sourceMappingURL=TableState.js.map