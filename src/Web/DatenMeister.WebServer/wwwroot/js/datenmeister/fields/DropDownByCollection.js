import { ObjectType } from "../Mof.js";
import * as ElementClient from "../client/Elements.js";
import { _DatenMeister } from "../models/DatenMeister.class.js";
import * as DropDownBaseField from "./DropDownBaseField.js";
import * as QueryBuilder from "../modules/QueryEngine.js";
export class Field extends DropDownBaseField.DropDownBaseField {
    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.References;
    }
    async loadFields() {
        const workspace = this.field.get(_DatenMeister._Forms._ReferenceFieldFromCollectionData.defaultWorkspace, ObjectType.String) ?? "Data";
        const path = this.field.get(_DatenMeister._Forms._ReferenceFieldFromCollectionData.collection, ObjectType.String);
        // Builds the query
        const queryBuilder = new QueryBuilder.QueryBuilder();
        QueryBuilder.getElementsByPath(queryBuilder, workspace, path);
        const serverResult = await ElementClient.queryObject(queryBuilder.queryStatement);
        return serverResult.result.map(x => {
            return {
                title: x.get("name", ObjectType.String) ?? x.id,
                workspace: x.workspace,
                itemUrl: x.uri
            };
        });
    }
}
//# sourceMappingURL=ReferenceFieldFromCollection.js.map