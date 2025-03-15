import * as Mof from "../Mof.js";
import * as DropDownBaseField from "./DropDownBaseField.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as ElementClient from "../client/Elements.js";
export class Field extends DropDownBaseField.DropDownBaseField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.Strings;
    }
    async loadFields() {
        const query = this.field.get(_DatenMeister._Forms._DropDownByQueryData.query, Mof.ObjectType.Single);
        // Checks, if the query is set, otherwise return an error message
        if (query === undefined || query === null) {
            return [
                {
                    title: "-- ERROR: Parameter Query is not set",
                    value: ""
                }
            ];
        }
        // Takes the query and executes the request
        const serverResult = await ElementClient.queryObject(query);
        return serverResult.result.map(x => {
            return {
                title: x.get("name", Mof.ObjectType.String) ?? x.id,
                workspace: x.workspace,
                itemUrl: x.uri
            };
        });
    }
}
//# sourceMappingURL=DropDownByQuery.js.map