import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as Mof from "../Mof.js"
import * as FormActions from "../FormActions.js"
import {expect} from "chai";

import '../../node_modules/chai/register-assert.js';
declare var assert: Chai.AssertStatic;


export function includeTests() {

    describe('ClientTests', function () {
        it('Render Html', async () => {
        
            const clientAction = new Mof.DmObject(_DatenMeister._Actions._ClientActions.__RenderHtmlClientAction_Uri);
            clientAction.set(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.html, "<div id='Test_Rendering'>Test</div>");
            
            await FormActions.executeClientAction(clientAction);
            
            const renderedText = $("#Test_Rendering").text();
            assert.isTrue(renderedText ==="Test");            
        });
        
        it('Render Text', async () => {

            $('#pageContent').empty();
            const clientAction = new Mof.DmObject(_DatenMeister._Actions._ClientActions.__RenderHtmlClientAction_Uri);
            clientAction.set(_DatenMeister._Actions._ClientActions._RenderHtmlClientAction.text, "<b>Test</b>");

            await FormActions.executeClientAction(clientAction);

            const renderedText = $("#pageContent").text();
            assert.isTrue(renderedText ==="<b>Test</b>");
        });
    });
}