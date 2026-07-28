// This file is generated in build.cake. Do NOT modify that file.
// Modify DatenMeister.Reports.Swimlane.Source.ts
// noinspection DuplicatedCode


import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces";
import {DmObject} from "/js/datenmeister/Mof.js";
import {SubmitMethod} from "/js/datenmeister/forms/Forms.js";
import * as DmModel from '/js/datenmeister/models/DatenMeister.class.js';

export function init()
{
    FormActions.addModule(new SwimlaneShowReportAction());    
}

class SwimlaneShowReportAction
    extends FormActions.ItemFormActionModuleBase
    implements FormActions.IItemFormActionModule 
{
    constructor() {
        super();
        this.actionName = "Reports.Swimlane.Show";
    }
    
    async execute(form?: IFormNavigation, element?: DmObject, parameter?: DmObject, submitMethod?: SubmitMethod): Promise<DmObject | void> {
        if(element === undefined) throw new Error("element is null"); 
            
        const createWindow = new DmObject(DmModel._Actions._ClientActions.__NavigateOpenActionInWindow_Uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.context, "render");
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.actionUrl, element.uri);
        createWindow.set(DmModel._Actions._ClientActions._NavigateOpenActionInWindow.workspaceId, element.workspace);        

        await FormActions.executeClientAction(createWindow);
        return Promise.resolve(undefined);
    }
}

// ---------------------------------------------------------------------------
// Swimlane rendering (decoupled from data loading so an offline static export
// can render a table by embedding the SwimlaneData directly in the page).
// ---------------------------------------------------------------------------

/**
 * A single item that belongs to one (horizontalKey, verticalKey) cell.
 */
export interface SwimlaneCellItem {
    /** Text to render for the item. */
    label: string;
    /** Optional link target — when set the label is rendered as an <a>. */
    link?: string;
}

/**
 * The data structure consumed by the renderer.
 *
 * The renderer never queries the server itself; the input is a fully-resolved
 * matrix. This keeps the renderer usable both from the live web page and from
 * an offline HTML export that carries its data as inlined JavaScript.
 */
export interface SwimlaneData {
    /** Label for the row/header column, e.g. "Assigned To". */
    horizontalHeader?: string;
    /** Column headers in display order (the vertical swimlanes). */
    verticalHeaders: string[];
    /** Row headers in display order (the horizontal swimlanes). */
    horizontalHeaders: string[];
    /**
     * Cell content keyed by "<horizontalHeader>|<verticalHeader>". Missing
     * entries render as an empty cell.
     */
    cells: { [key: string]: SwimlaneCellItem[] };
    /** Optional title rendered above the table. */
    title?: string;
}

export function cellKey(horizontalHeader: string, verticalHeader: string): string {
    return horizontalHeader + "|" + verticalHeader;
}

/**
 * Renders the swimlane into the given container. The container is emptied
 * before rendering.
 */
export function renderInto(container: JQuery<HTMLElement>, data: SwimlaneData): void {
    container.empty();

    if (data.title !== undefined && data.title !== "") {
        container.append($("<h2></h2>").text(data.title));
    }

    const table = $("<table class='table table-bordered dm-swimlane-table'></table>");
    const thead = $("<thead></thead>");
    const headerRow = $("<tr></tr>");
    headerRow.append($("<th></th>").text(data.horizontalHeader ?? ""));
    for (const vertical of data.verticalHeaders) {
        headerRow.append($("<th></th>").text(vertical));
    }
    thead.append(headerRow);
    table.append(thead);

    const tbody = $("<tbody></tbody>");
    for (const horizontal of data.horizontalHeaders) {
        const row = $("<tr></tr>");
        row.append($("<th scope='row'></th>").text(horizontal));

        for (const vertical of data.verticalHeaders) {
            const cell = $("<td></td>");
            const items = data.cells[cellKey(horizontal, vertical)] ?? [];
            for (let i = 0; i < items.length; i++) {
                const item = items[i];
                if (i > 0) {
                    cell.append($("<br/>"));
                }
                if (item.link !== undefined && item.link !== "") {
                    cell.append($("<a></a>").attr("href", item.link).text(item.label));
                } else {
                    cell.append(document.createTextNode(item.label));
                }
            }
            row.append(cell);
        }

        tbody.append(row);
    }
    table.append(tbody);
    container.append(table);
}

// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export namespace _Root
{
        export class _SwimlaneConfiguration
        {
            static verticalSwimlaneProperty = "verticalSwimlaneProperty";
            static horizontalSwimlaneProperty = "horizontalSwimlaneProperty";
            static cellContent = "cellContent";
            static linkContent = "linkContent";
            static _name_ = "name";
        }

        export const __SwimlaneConfiguration_Uri = "dm:///intern.types.swimlane.datenmeister/#e92b2227-9b93-4033-ac2e-772a2230a869";
        export class _SwimlaneViewDefinition
        {
            static swimlaneConfiguration = "swimlaneConfiguration";
            static viewNode = "viewNode";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __SwimlaneViewDefinition_Uri = "dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c";
}

