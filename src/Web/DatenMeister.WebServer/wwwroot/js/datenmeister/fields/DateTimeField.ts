
import { BaseField, IFormField } from "./Interfaces.js";
import * as Mof from "../Mof.js";
import * as _DatenMeister from "../models/DatenMeister.class.js";

export class Field extends BaseField implements IFormField {
    _year: JQuery<HTMLElement>;
    _month: JQuery<HTMLElement>;
    _day: JQuery<HTMLElement>;
    _hour: JQuery<HTMLElement>;
    _minute: JQuery<HTMLElement>;

    async createDom(dmElement: Mof.DmObject): Promise<JQuery<HTMLElement>> {
        const fieldName = this.field.get('name')?.toString() ?? "";
        const hideDate = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideDate, Mof.ObjectType.Boolean) ?? false;
        const hideTime = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideTime, Mof.ObjectType.Boolean) ?? false;

        const value = dmElement.get(fieldName, Mof.ObjectType.String) ?? "";
        let dateObj: Date | null = null;
        if (value) {
            dateObj = new Date(value);
            if (isNaN(dateObj.getTime())) {
                dateObj = null;
            }
        }

        if (this.isReadOnly) {
            const container = $("<div />");
            if (!dateObj) {
                container.append($("<em class='dm-undefined'>undefined</em>"));
                return container;
            }

            const pad = (n: number) => n < 10 ? '0' + n : n;
            let text = "";
            if (!hideDate) {
                text += `${dateObj.getFullYear()}-${pad(dateObj.getMonth() + 1)}-${pad(dateObj.getDate())}`;
            }

            if (!hideTime) {
                if (text !== "") text += " ";
                text += `${pad(dateObj.getHours())}:${pad(dateObj.getMinutes())}`;
            }

            container.text(text);
            return container;
        }

        const container = $("<div class='align-items-center' />");

        if (!hideDate) {
            container.append($("<span>Date: </span>"));
            const dateInputs = $("<div class='d-flex' />");
            this._year = $("<input type='number' class='form-control me-1' style='width: 80px' placeholder='YYYY' />");
            this._month = $("<input type='number' class='form-control me-1' style='width: 60px' placeholder='MM' min='1' max='12' />");
            this._day = $("<input type='number' class='form-control me-1' style='width: 60px' placeholder='DD' min='1' max='31' />");

            if (dateObj) {
                this._year.val(dateObj.getFullYear());
                this._month.val(dateObj.getMonth() + 1);
                this._day.val(dateObj.getDate());
            }

            dateInputs.append(this._year, this._month, this._day);
            container.append(dateInputs);
        }

        if (!hideTime) {
            container.append($("<span>Time: </span>"));
            const timeInputs = $("<div class='d-flex align-items-center' />");
            this._hour = $("<input type='number' class='form-control me-1' style='width: 60px' placeholder='HH' min='0' max='23' />");
            this._minute = $("<input type='number' class='form-control me-1' style='width: 60px' placeholder='MM' min='0' max='59' />");

            if (dateObj) {
                this._hour.val(dateObj.getHours());
                this._minute.val(dateObj.getMinutes());
            }

            timeInputs.append(this._hour, $("<span class='me-1'>:</span>"), this._minute);
            container.append(timeInputs);
        }

        if (!this.isReadOnly) {
            const nowBtn = $("<button class='btn btn-outline-secondary btn-sm'>Now</button>");
            nowBtn.on('click', () => {
                const now = new Date();
                if (this._year) this._year.val(now.getFullYear());
                if (this._month) this._month.val(now.getMonth() + 1);
                if (this._day) this._day.val(now.getDate());
                if (this._hour) this._hour.val(now.getHours());
                if (this._minute) this._minute.val(now.getMinutes());
                
                if (this.callbackUpdateField) this.callbackUpdateField();
            });
            container.append(nowBtn);
            
            const changeHandler = () => {
                if (this.callbackUpdateField) this.callbackUpdateField();
            };
            
            if (this._year) this._year.on('change', changeHandler);
            if (this._month) this._month.on('change', changeHandler);
            if (this._day) this._day.on('change', changeHandler);
            if (this._hour) this._hour.on('change', changeHandler);
            if (this._minute) this._minute.on('change', changeHandler);
        }

        return container;
    }

    async evaluateDom(dmElement: Mof.DmObject): Promise<void> {
        const fieldName = this.field.get('name')?.toString() ?? "";
        const hideDate = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideDate, Mof.ObjectType.Boolean) ?? false;
        const hideTime = this.field.get(_DatenMeister._Forms._DateTimeFieldData.hideTime, Mof.ObjectType.Boolean) ?? false;

        let year = 1;
        let month = 1;
        let day = 1;
        let hour = 0;
        let minute = 0;

        if (!hideDate && this._year && this._month && this._day) {
            year = parseInt(this._year.val()?.toString() ?? "1");
            month = parseInt(this._month.val()?.toString() ?? "1") - 1;
            day = parseInt(this._day.val()?.toString() ?? "1");
        }

        if (!hideTime && this._hour && this._minute) {
            hour = parseInt(this._hour.val()?.toString() ?? "0");
            minute = parseInt(this._minute.val()?.toString() ?? "0");
        }
        
        // If everything is empty, we might want to set it to undefined/null, 
        // but for now we follow the requirement: "The data is stored as a string, 
        // which can be read by converting the String to a .Net DateTime value."
        
        const date = new Date(year, month, day, hour, minute);
        if (!isNaN(date.getTime())) {
            // ISO format is usually readable by .NET DateTime.Parse
            // We want to make sure it's a local time as requested.
            // .toISOString() returns UTC.
            // Let's manually construct a string that .NET likes.
            const pad = (n: number) => n < 10 ? '0' + n : n;
            const formatted = `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:00`;
            dmElement.set(fieldName, formatted);
        }
    }
}
