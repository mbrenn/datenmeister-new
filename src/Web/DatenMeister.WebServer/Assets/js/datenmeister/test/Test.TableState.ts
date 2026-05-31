import * as _DatenMeister from "../models/DatenMeister.class.js";
import { TableState } from "../forms/TableState.js";

import {_DataViews} from "../models/DatenMeister.class.js";
declare var expect: Chai.ExpectStatic;

export function includeTests() {
    describe('Forms', () => {
        describe('TableState', () => {
            it('Initialize TableState', () => {
                const tableState = new TableState();
                tableState.initialize();

                expect(tableState.queryStatement).to.not.be.null;
                const nodes = tableState.queryStatement.getAsArray(_DatenMeister._DataViews._QueryStatement.nodes);
                // By default it should have a dynamic source and a limit
                expect(nodes.length).to.equal(2);
            });

            it('Set and Get OrderBy', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setOrderBy("name", true);
                let orderBy = tableState.getOrderBy();
                expect(orderBy).to.not.be.undefined;
                expect(orderBy.property).to.equal("name");
                expect(orderBy.descending).to.be.true;

                tableState.setOrderBy("age", false);
                orderBy = tableState.getOrderBy();
                expect(orderBy.property).to.equal("age");
                expect(orderBy.descending).to.be.false;

                tableState.removeOrderBy();
                expect(tableState.getOrderBy()).to.be.undefined;
            });

            it('Set and Get FreeTextFilter', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setFreeTextFilter("search term");
                expect(tableState.getFreeTextFilter()).to.equal("search term");

                tableState.setFreeTextFilter("new search");
                expect(tableState.getFreeTextFilter()).to.equal("new search");

                tableState.removeFreeTextFilter();
                expect(tableState.getFreeTextFilter()).to.be.undefined;
            });

            it('Set and Get FilterByProperty', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setFilterByProperty("category", "electronics");
                expect(tableState.getFilterByProperty("category")).to.equal("electronics");
                
                tableState.setFilterByProperty("other", "valid");
                expect(tableState.getFilterByProperty("other")).to.equal("valid");

                tableState.setFilterByProperty("category", "books");
                expect(tableState.getFilterByProperty("category")).to.equal("books");

                tableState.removeFilterByProperty("category");
                expect(tableState.getFilterByProperty("category")).to.be.undefined;
            });

            it('Set and Get Limit', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setLimit(50);
                expect(tableState.getLimit()).to.equal(50);

                tableState.setLimit(200);
                expect(tableState.getLimit()).to.equal(200);

                tableState.removeLimit();
                expect(tableState.getLimit()).to.be.undefined;
            });

            it('Get FilterByProperties', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setFilterByProperty("prop1", "val1");
                tableState.setFilterByProperty("prop2", "val2");

                const filters = tableState.getFilterByProperties();
                expect(filters["prop1"]).to.equal("val1");
                expect(filters["prop2"]).to.equal("val2");
            });

            it('Complex Node Manipulation', () => {
                const tableState = new TableState();
                tableState.initialize();

                tableState.setOrderBy("name", true);
                tableState.setFreeTextFilter("test");
                tableState.setLimit(10);

                expect(tableState.getOrderBy().property).to.equal("name");
                expect(tableState.getFreeTextFilter()).to.equal("test");
                expect(tableState.getLimit()).to.equal(10);

                tableState.removeOrderBy();
                expect(tableState.getOrderBy()).to.be.undefined;
                expect(tableState.getFreeTextFilter()).to.equal("test");
                expect(tableState.getLimit()).to.equal(10);
            });
            
            it('Check that Limit Node is always the first node', () => {
                const tableState = new TableState();
                tableState.initialize();
                
                // Per default, the first node shall be of type limit node
                let viewNodes = tableState.getViewNodes();
                expect(viewNodes[0].metaClass.uri).to.equal(_DataViews._Row.__RowFilterOnPositionNode_Uri);
                
                tableState.setOrderBy("Test", true);
                viewNodes = tableState.getViewNodes();
                expect(viewNodes[0].metaClass.uri).to.equal(_DataViews._Row.__RowFilterOnPositionNode_Uri);

                tableState.setFreeTextFilter("test");
                viewNodes = tableState.getViewNodes();
                expect(viewNodes[0].metaClass.uri).to.equal(_DataViews._Row.__RowFilterOnPositionNode_Uri);
                
                tableState.setFilterByProperty("Test", "test");
                viewNodes = tableState.getViewNodes();
                expect(viewNodes[0].metaClass.uri).to.equal(_DataViews._Row.__RowFilterOnPositionNode_Uri);                
            });
        });
    });
}

// Auto-run when executed directly under Node/Mocha
// @ts-ignore
if (typeof window === 'undefined') {
    includeTests();
}
