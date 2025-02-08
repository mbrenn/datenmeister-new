import * as QueryBuilder from '../modules/QueryEngine.js';
import * as ElementClient from '../client/Elements.js';
import * as DomHelper from '../DomHelper.js'
export class Control {

    _control: JQuery;

    /**
     * 
     * Initializes the control and adds all necessary elements to the container
     * @param container
     */
    init(container: JQuery) {

        const tthis = this;

        this._control = $("<div>" +
            "<h1>Path Query Debug</h1>" +
            "<div>Workspace: <input type='text' class='dm-pathquery-debug-workspace' /></div>" +
            "<div>Path: <input type='text' class='dm-pathquery-debug-path' /></div>" +
            "<button class='dm-pathquery-debug-query'>Query</button>" +
            "<div class='dm-pathquery-debug-result'></div>" +
            "< /div>");

        // Add click handler to the button        
        const queryButton = this._control.find('.dm-pathquery-debug-query');
        queryButton.on('click', async () => {
            const pathWorkspace = this._control.find('.dm-pathquery-debug-workspace').val().toString();
            const pathInput = this._control.find('.dm-pathquery-debug-path').val().toString();
            await tthis.performQuery(pathWorkspace, pathInput);
        });

        container.append(this._control);
    }

    /**
     * Executes the query and shows the result directly within the field.
     * @param workspace
     * @param path
     */

    private async performQuery(workspace: string, path: string) {
        const result = this._control.find('.dm-pathquery-debug-result');
        result.empty();

        var queryBuilder = new QueryBuilder.QueryBuilder();
        QueryBuilder.getElementsOfExtent(queryBuilder, workspace, path);

        const serverResult = await ElementClient.queryObject(queryBuilder.queryStatement);
        result.append(DomHelper.convertToDom(serverResult));
        serverResult.result;
    }
}