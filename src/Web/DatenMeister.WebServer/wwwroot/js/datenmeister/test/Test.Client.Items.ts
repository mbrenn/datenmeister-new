import * as ClientExtent from "../Client.Extents"
import * as ClientWorkspace from "../Client.Workspace"
import * as ClientItems from "../Client.Items"
import {DmObject} from "../Mof";

export function includeTests() {
    describe('Client', function () {
        describe('Items', function () {

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
            });
            
            it('Create and Delete Item', async function(){
                try {
                    
                }
                catch(e)
                {
                    throw e;
                }
            });         
            

            after(async function () {                
                await ClientExtent.deleteExtent({
                        workspace: "Test",
                        extentUri: "dm:///unittest",
                        skipIfNotExisting: true
                    });
                
                await ClientExtent.deleteExtent({
                    workspace: "Test",
                    extentUri: "dm:///newexisting",
                    skipIfNotExisting: true
                });
                
                await ClientWorkspace.deleteWorkspace("Test");
            });
        });
    });
}