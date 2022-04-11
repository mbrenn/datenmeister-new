import * as ClientWorkspace from "../client/Workspace";
import * as ClientExtent from "../client/Extents";
import * as ClientItems from "../client/Items";
import {SelectItemControl} from "../controls/SelectItemControl";


export function includeTests() {
    describe('Controls',
        () => {

            before(async function () {
                await ClientWorkspace.createWorkspace(
                    "Test",
                    "Annotation",
                    {skipIfExisting: true});

                await ClientExtent.createXmi(
                    {
                        extentUri: "dm:///unittest",
                        filePath: "./unittest.xmi",
                        workspace: "Test",
                        skipIfExisting: true
                    });


                await ClientItems.deleteRootElements("Test", "dm:///unittest");
            });

            describe('SelectItemControl',
                () => {
                    it('Create Controls and Load Workspaces',
                        async () => {
                            let div = $("#dom_test");
                            if (div === undefined) {
                                div = $("<div></div>");
                            }

                            const sic = new SelectItemControl();
                            const query = await sic.initAsync(div);

                            const workspace = $("#dm-sic-workspace select", query);
                            chai.assert.isTrue(workspace !== undefined, "No select given");

                            const children = workspace.children();
                            let found = false;
                            children.each((index, child) => {
                                if ($(child).text() === "abc") {
                                    found = true;
                                }
                            });

                            chai.assert.isTrue(found, "Workspace was not found");


                            div.remove();

                        });
                });
        });

}
