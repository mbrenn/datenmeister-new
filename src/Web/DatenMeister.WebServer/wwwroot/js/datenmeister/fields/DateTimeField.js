import { BaseField } from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";
export class Field extends BaseField {
    async createDom(dmElement) {
        const fieldName = this.field.get('name')?.toString() ?? "";
        const hideDate = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideDate, Mof.ObjectType.Boolean) ?? false;
        const hideTime = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideTime, Mof.ObjectType.Boolean) ?? false;
        const value = dmElement.get(fieldName, Mof.ObjectType.String) ?? "";
        let dateObj = null;
        if (value) {
            // Check if the value is an ISO string. If it doesn't have a timezone, append 'Z' to treat as UTC
            // or let the browser decide. 
            // The previous version stored as YYYY-MM-DDTHH:mm:ss which is usually treated as local by browsers
            // if not specified otherwise.
            if (value.indexOf('T') !== -1 && value.indexOf('Z') === -1 && value.indexOf('+') === -1) {
                dateObj = new Date(value + "Z"); // Treat as UTC if no offset
            }
            else {
                dateObj = new Date(value);
            }
            if (isNaN(dateObj.getTime())) {
                dateObj = null;
            }
        }
        const pad = (n) => n < 10 ? '0' + n : n;
        if (this.isReadOnly) {
            const container = $("<div />");
            if (!dateObj) {
                container.append($("<em class='dm-undefined'>undefined</em>"));
                return container;
            }
            let text = "";
            if (!hideDate) {
                text += `${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`;
            }
            if (!hideTime) {
                if (text !== "")
                    text += " ";
                text += `${pad(dateObj.getHours())}:${pad(dateObj.getMinutes())}`;
            }
            container.text(text);
            return container;
        }
        const container = $("<div />");
        if (!hideDate) {
            const dateContainer = $("<div class='d-flex align-items-center' />");
            dateContainer.append($("<span class='me-1'>Date: </span>"));
            this._dateInput = $("<input type='date' class='form-control' />");
            if (dateObj) {
                this._dateInput.val(`${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`);
            }
            dateContainer.append(this._dateInput);
            container.append(dateContainer);
        }
        if (!hideDate && !hideTime) {
            container.append($("<br/>"));
        }
        if (!hideTime) {
            const timeContainer = $("<div class='d-flex align-items-center' />");
            timeContainer.append($("<span class='me-1'>Time: </span>"));
            this._timeInput = $("<input type='time' class='form-control' />");
            if (dateObj) {
                this._timeInput.val(`${pad(dateObj.getHours())}:${pad(dateObj.getMinutes())}`);
            }
            timeContainer.append(this._timeInput);
            container.append(timeContainer);
        }
        if (!this.isReadOnly) {
            const nowBtn = $("<button class='btn btn-outline-secondary btn-sm mt-1'>Now</button>");
            nowBtn.on('click', () => {
                const now = new Date();
                if (this._dateInput) {
                    this._dateInput.val(`${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`);
                }
                if (this._timeInput) {
                    this._timeInput.val(`${pad(now.getHours())}:${pad(now.getMinutes())}`);
                }
                if (this.callbackUpdateField)
                    this.callbackUpdateField();
            });
            container.append(nowBtn);
            const changeHandler = () => {
                if (this.callbackUpdateField)
                    this.callbackUpdateField();
            };
            if (this._dateInput)
                this._dateInput.on('change', changeHandler);
            if (this._timeInput)
                this._timeInput.on('change', changeHandler);
        }
        return container;
    }
    async evaluateDom(dmElement) {
        const fieldName = this.field.get('name')?.toString() ?? "";
        const datePart = this._dateInput?.val()?.toString() || "";
        const timePart = this._timeInput?.val()?.toString() || "";
        if (!datePart && !timePart) {
            return;
        }
        // Use current date/time if parts are missing
        const now = new Date();
        const year = datePart ? parseInt(datePart.substring(0, 4)) : now.getFullYear();
        const month = datePart ? parseInt(datePart.substring(5, 7)) - 1 : now.getMonth();
        const day = datePart ? parseInt(datePart.substring(8, 10)) : now.getDate();
        const hours = timePart ? parseInt(timePart.substring(0, 2)) : 0;
        const minutes = timePart ? parseInt(timePart.substring(3, 5)) : 0;
        // Create date object from local input
        const dateObj = new Date(year, month, day, hours, minutes, 0);
        // Store as UTC ISO string
        dmElement.set(fieldName, dateObj.toISOString());
    }
}
//# sourceMappingURL=DateTimeField.js.map