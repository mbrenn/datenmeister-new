import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import { StaticObjectQuery } from "../modules/StaticObjectQuery.js";
import { TableState } from "../forms/TableState.js";
import * as QueryEngine from "../modules/QueryEngine.js";

import '../../node_modules/chai/register-expect.js';
declare var expect: Chai.ExpectStatic;

export function includeTests() {
    describe('Modules', () => {
        describe('StaticObjectQuery', () => {
            function createSampleObjects(): Mof.DmObject[] {
                const item1 = new Mof.DmObject("dm:///_types#TypeA");
                item1.set("name", "Alice");
                item1.set("age", 30);
                item1.set("role", "Developer");
                item1.set("active", true);

                const item2 = new Mof.DmObject("dm:///_types#TypeA");
                item2.set("name", "Bob");
                item2.set("age", 25);
                item2.set("role", "Manager");
                item2.set("active", false);

                const item3 = new Mof.DmObject("dm:///_types#TypeB");
                item3.set("name", "Charlie");
                item3.set("age", 35);
                item3.set("role", "Developer");
                item3.set("active", true);

                const item4 = new Mof.DmObject("dm:///_types#TypeB");
                item4.set("name", "Diana");
                item4.set("age", 28);
                item4.set("role", "Designer");
                item4.set("active", false);

                return [item1, item2, item3, item4];
            }

            it('No filter returns all items without mutating original', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1); // No limit

                const result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(4);
                expect(result).to.not.equal(original); // Must be a new array
            });

            it('Filters by free text across all properties', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);
                tableState.setFreeTextFilter("dev");

                const result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Alice", "Charlie"]);
            });

            it('Filters by property value (Equal)', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);
                tableState.setFilterByProperty("role", "Developer");

                const result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Alice", "Charlie"]);
            });

            it('Filters by multiple property values', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);
                tableState.setFilterByProperty("role", "Developer");
                tableState.setFilterByProperty("name", "Charlie");

                const result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(1);
                expect(result[0].get("name")).to.equal("Charlie");
            });

            it('Filters by comparison modes (Contains, GreaterThan, etc.)', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);

                const builder = new QueryEngine.QueryBuilder();
                QueryEngine.addDynamicSource(builder, "input");
                QueryEngine.filterByProperty(builder, "name", "li", _DatenMeister._DataViews.___ComparisonMode.Contains);

                const result = query.getFilteredObject(builder.queryStatement);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Alice", "Charlie"]);
            });

            it('Filters by metaclass', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);

                const builder = new QueryEngine.QueryBuilder();
                QueryEngine.addDynamicSource(builder, "input");
                const metaTypeB = Mof.DmObject.createFromReference("Types", "dm:///_types#TypeB");
                QueryEngine.filterByMetaClass(builder, metaTypeB);

                const result = query.getFilteredObject(builder.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Charlie", "Diana"]);
            });

            it('Sorts by string property ascending and descending', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);

                tableState.setOrderBy("name", false);
                let result = query.getFilteredObject(tableState.queryStatement);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Alice", "Bob", "Charlie", "Diana"]);

                tableState.setOrderBy("name", true);
                result = query.getFilteredObject(tableState.queryStatement);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Diana", "Charlie", "Bob", "Alice"]);
            });

            it('Sorts by numeric property ascending and descending', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);

                tableState.setOrderBy("age", false);
                let result = query.getFilteredObject(tableState.queryStatement);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Bob", "Diana", "Alice", "Charlie"]);

                tableState.setOrderBy("age", true);
                result = query.getFilteredObject(tableState.queryStatement);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Charlie", "Alice", "Diana", "Bob"]);
            });

            it('Paginates results using limit and position', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(2);

                let result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Alice", "Bob"]);

                // Set limit with position offset
                const builder = new QueryEngine.QueryBuilder();
                QueryEngine.addDynamicSource(builder, "input");
                const limitNode = QueryEngine.limit(builder, 2);
                limitNode.set(_DatenMeister._DataViews._Row._RowFilterOnPositionNode.position, 1);

                result = query.getFilteredObject(builder.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Bob", "Charlie"]);
            });

            it('Applies combined filter, sort, and pagination pipeline', () => {
                const original = createSampleObjects();
                const query = new StaticObjectQuery(original);
                const tableState = new TableState();
                tableState.initialize(-1);

                tableState.setFreeTextFilter("e"); // Matches Alice, Charlie, Developer, Manager, Designer -> Alice, Bob, Charlie, Diana
                tableState.setOrderBy("age", true); // Charlie (35), Alice (30), Diana (28), Bob (25)
                tableState.setLimit(2); // Top 2: Charlie, Alice

                const result = query.getFilteredObject(tableState.queryStatement);
                expect(result.length).to.equal(2);
                expect(result.map(x => x.get("name"))).to.deep.equal(["Charlie", "Alice"]);
            });
        });
    });
}

// Auto-run when executed directly under Node/Mocha
// @ts-ignore
if (typeof window === 'undefined') {
    includeTests();
}
