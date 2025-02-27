import {BaseField, IFormField} from "./Interfaces.js";
import {DmObject, ObjectType} from "../Mof.js";
import * as ElementClient from "../client/Elements.js"
import { _DatenMeister } from "../models/DatenMeister.class.js";

import * as DropDownBaseField from "./DropDownBaseField.js";
import * as QueryBuilder from "../modules/QueryEngine.js";
export class Field extends DropDownBaseField.DropDownBaseField implements IFormField {

    constructor() {
        super();
        this.fieldType = DropDownBaseField.FieldType.References;
    }

    async loadFields(): Promise<DropDownBaseField.DropDownOptionField[]> {
        const workspace = this.field.get(
            _DatenMeister._Forms._DropDownByCollection.defaultWorkspace, ObjectType.String) ?? "Data";
        const path = this.field.get(
            _DatenMeister._Forms._DropDownByCollection.collection, ObjectType.String)

        // Builds the query
        const queryBuilder = new QueryBuilder.QueryBuilder();
        QueryBuilder.getElementsByPath(queryBuilder, workspace, path);

        const serverResult = await ElementClient.queryObject(queryBuilder.queryStatement);

        return serverResult.result.map(x => {
            return {
                title: x.get("name", ObjectType.String) ?? x.id,
                workspace: x.workspace,
                itemUrl: x.uri
            }
        });
    }
}