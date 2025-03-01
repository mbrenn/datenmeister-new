import {IFormField} from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as DropDownBaseField from "./DropDownBaseField.js";
import {_DatenMeister} from "../models/DatenMeister.class.js";
import * as ElementClient from "../client/Elements.js";

export class Field extends DropDownBaseField.DropDownBaseField implements IFormField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.Strings;
    }

    async loadFields(): Promise<DropDownBaseField.DropDownOptionField[]> {
        const query = this.field.get(
            _DatenMeister._Forms._DropDownByQueryData.query, Mof.ObjectType.Single);        
        
        // Checks, if the query is set, otherwise return an error message
        if (query === undefined || query === null)
        {
            return [
                {
                    title: "-- ERROR: Parameter Query is not set",
                    value: ""
                }
            ];
        }

        // Takes the query and executes the request
        const serverResult = await ElementClient.queryObject(
            Mof.DmObject.createFromReference(query.workspace, query.uri));

        return serverResult.result.map(x => {
            return {
                title: x.get("name", Mof.ObjectType.String) ?? x.id,
                workspace: x.workspace,
                itemUrl: x.uri
            }
        });
    }    
}