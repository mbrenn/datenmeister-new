import * as VML from "../forms/ViewModeLogic.js";


export function includeTests() {
    describe('ViewModeLogic', () => {
        it('Clear, get, set, get, clear, get', () => 
        {   
            VML.clearCurrentViewMode();
            let viewMode = VML.getCurrentViewMode();            
            chai.assert.isTrue(viewMode === "ViewMode.Default", "viewMode needs to be 'Default'");
            
            VML.setCurrentViewMode('Test');
            viewMode = VML.getCurrentViewMode();
            chai.assert.isTrue(viewMode === "Test", "viewMode needs to be 'Test'");

            VML.clearCurrentViewMode();
            viewMode = VML.getCurrentViewMode();
            chai.assert.isTrue(viewMode === "ViewMode.Default", "viewMode needs to be 'Default'");
        });
    });
}