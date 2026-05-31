import { BaseField } from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
import * as FieldFactory from "../forms/FieldFactory.js";
export class Field extends BaseField {
    childFields = [];
    /**
     * Inhibits the presentation of the checkbox indicating whether the element is set.
     */
    showValue() { return false; }
    async createDom(dmElement) {
        const fields = this.field.get(_DatenMeister._Forms._FieldTypes._MergedFieldsInCellData.fields, Mof.ObjectType.Array) ?? [];
        const container = $("<div class='dm-merged-fields-in-cell d-flex flex-wrap gap-2 align-items-center'></div>");
        this.childFields = [];
        for (const field of fields) {
            if (!(field instanceof Mof.DmObject)) {
                continue;
            }
            const fieldMetaClassUri = field.metaClass?.uri;
            const isFieldReadOnly = this.isReadOnly ||
                field.get(_DatenMeister._Forms._FieldTypes._FieldData.isReadOnly, Mof.ObjectType.Boolean);
            const fieldElement = FieldFactory.createField(fieldMetaClassUri, {
                configuration: this.configuration,
                field: field,
                itemUrl: this.itemUrl,
                isReadOnly: isFieldReadOnly,
                form: this.form
            });
            fieldElement.callbackUpdateField = () => {
                if (this.callbackUpdateField !== undefined) {
                    this.callbackUpdateField();
                }
            };
            this.childFields.push(fieldElement);
            container.append(await fieldElement.createDom(dmElement));
        }
        return container;
    }
    async evaluateDom(dmElement) {
        for (const field of this.childFields) {
            await field.evaluateDom(dmElement);
        }
    }
}
export class Control extends Field {
}
//# sourceMappingURL=MergedFieldsInCell.js.map