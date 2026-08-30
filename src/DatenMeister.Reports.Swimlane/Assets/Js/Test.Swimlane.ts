import * as Mof from "/js/datenmeister/Mof.js";
import {buildSwimlaneData, cellKey, _Root} from "./DatenMeister.Reports.Swimlane.js";

import '../../../Web/DatenMeister.WebServer/node_modules/chai/register-expect.js';
declare var expect: Chai.ExpectStatic;

export function includeTests() {
    describe('Reports', () => {
        describe('Swimlane', () => {
            function createSampleTasks(): Mof.DmObject[] {
                const tasks = [
                    { task: "Fix login bug", sprint: "Sprint 1", assignedTo: "Alice" },
                    { task: "Write unit tests", sprint: "Sprint 1", assignedTo: "Bob" },
                    { task: "Implement search", sprint: "Sprint 2", assignedTo: "Alice" },
                    { task: "Code review", sprint: "Sprint 2", assignedTo: "Alice" },
                    { task: "Deploy to staging", sprint: "Sprint 2", assignedTo: "Bob" },
                    { task: "Update documentation", sprint: "Sprint 3", assignedTo: "Carol" },
                    { task: "Performance tuning", sprint: "Sprint 3", assignedTo: "Bob" },
                    { task: "Fix export issue", sprint: "Sprint 3", assignedTo: "Carol" }
                ];

                return tasks.map(t => {
                    const obj = new Mof.DmObject("dm:///_types#Task");
                    obj.set("name", t.task);
                    obj.set("Sprint", t.sprint);
                    obj.set("Assigned To", t.assignedTo);
                    return obj;
                });
            }

            function createSwimlaneConfig(cellContent?: string, linkContent?: boolean): Mof.DmObject {
                const config = new Mof.DmObject(_Root.__SwimlaneConfiguration_Uri);
                config.set(_Root._SwimlaneConfiguration.verticalSwimlaneProperty, "Sprint");
                config.set(_Root._SwimlaneConfiguration.horizontalSwimlaneProperty, "Assigned To");
                if (cellContent !== undefined) {
                    config.set(_Root._SwimlaneConfiguration.cellContent, cellContent);
                }
                if (linkContent !== undefined) {
                    config.set(_Root._SwimlaneConfiguration.linkContent, linkContent);
                }
                return config;
            }

            it('Builds correct swimlane rows, columns and cell clustering', () => {
                const tasks = createSampleTasks();
                const config = createSwimlaneConfig();

                const data = buildSwimlaneData(tasks, config, "Sprint Overview");

                expect(data.title).to.equal("Sprint Overview");
                expect(data.horizontalHeader).to.equal("Assigned To");
                expect(data.verticalHeaders).to.deep.equal(["Sprint 1", "Sprint 2", "Sprint 3"]);
                expect(data.horizontalHeaders).to.deep.equal(["Alice", "Bob", "Carol"]);

                // Alice's tasks
                const aliceSprint1 = data.cells[cellKey("Alice", "Sprint 1")];
                expect(aliceSprint1).to.have.lengthOf(1);
                expect(aliceSprint1[0].label).to.equal("Fix login bug");

                const aliceSprint2 = data.cells[cellKey("Alice", "Sprint 2")];
                expect(aliceSprint2).to.have.lengthOf(2);
                expect(aliceSprint2.map(x => x.label)).to.deep.equal(["Implement search", "Code review"]);

                const aliceSprint3 = data.cells[cellKey("Alice", "Sprint 3")];
                expect(aliceSprint3).to.be.undefined;

                // Bob's tasks
                const bobSprint1 = data.cells[cellKey("Bob", "Sprint 1")];
                expect(bobSprint1).to.have.lengthOf(1);
                expect(bobSprint1[0].label).to.equal("Write unit tests");

                const bobSprint2 = data.cells[cellKey("Bob", "Sprint 2")];
                expect(bobSprint2).to.have.lengthOf(1);
                expect(bobSprint2[0].label).to.equal("Deploy to staging");

                const bobSprint3 = data.cells[cellKey("Bob", "Sprint 3")];
                expect(bobSprint3).to.have.lengthOf(1);
                expect(bobSprint3[0].label).to.equal("Performance tuning");

                // Carol's tasks
                const carolSprint1 = data.cells[cellKey("Carol", "Sprint 1")];
                expect(carolSprint1).to.be.undefined;

                const carolSprint3 = data.cells[cellKey("Carol", "Sprint 3")];
                expect(carolSprint3).to.have.lengthOf(2);
                expect(carolSprint3.map(x => x.label)).to.deep.equal(["Update documentation", "Fix export issue"]);
            });

            it('Renders cell content using template', () => {
                const tasks = createSampleTasks();
                const config = createSwimlaneConfig("Task: {{ name }}");

                const data = buildSwimlaneData(tasks, config);
                const aliceSprint1 = data.cells[cellKey("Alice", "Sprint 1")];
                expect(aliceSprint1[0].label).to.equal("Task: Fix login bug");
            });

            it('Generates links when linkContent is true', () => {
                const task = new Mof.DmObject("dm:///_types#Task");
                task.set("name", "Fix login bug");
                task.set("Sprint", "Sprint 1");
                task.set("Assigned To", "Alice");
                task.uri = "dm:///tasks#task1";
                task.workspace = "Data";

                const config = createSwimlaneConfig(undefined, true);
                const data = buildSwimlaneData([task], config);

                const cell = data.cells[cellKey("Alice", "Sprint 1")];
                expect(cell).to.have.lengthOf(1);
                expect(cell[0].link).to.not.be.undefined;
                expect(cell[0].link).to.include("task1");
            });
        });
    });
}

// Auto-run when executed directly under Node/Mocha
// @ts-ignore
if (typeof window === 'undefined') {
    includeTests();
}
