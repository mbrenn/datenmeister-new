
import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces.js";
import * as Mof from "/js/datenmeister/Mof.js";
import {DmObject} from "/js/datenmeister/Mof.js";
import {SubmitMethod} from "/js/datenmeister/forms/Forms.js";
import * as DmModel from '/js/datenmeister/models/DatenMeister.class.js';
import * as FormInterfaces from '/js/datenmeister/forms/Interfaces.js';
import * as FormFactory from "/js/datenmeister/forms/FormFactory.js";
import {IDecoupledFormConfiguration} from "/js/datenmeister/forms/IFormConfiguration.js";
import * as QueryEngine from "/js/datenmeister/modules/QueryEngine.js";
import * as ClientElements from "/js/datenmeister/client/Elements.js";
import * as ClientItems from "/js/datenmeister/client/Items.js";
import * as Navigator from "/js/datenmeister/Navigator.js";

export function init()
{
    FormActions.addModule(new SwimlaneShowReportAction());
    FormFactory.registerDecoupledForm(_Root.__SwimlaneForm_Uri, () => new SwimlaneForm());
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

class SwimlaneForm implements FormInterfaces.IDecoupledForm
{
    async createForm(parent: JQuery<HTMLElement>, configuration: IDecoupledFormConfiguration): Promise<void> {
        let swimlaneViewDefinition = configuration.form.get(_Root._SwimlaneForm.swimlaneViewDefinition, Mof.ObjectType.Object);
        if (swimlaneViewDefinition === undefined) {
            throw new Error("SwimlaneViewDefinition is undefined");
        }

        // If swimlaneViewDefinition is a reference without loaded properties, fetch it from server
        if (swimlaneViewDefinition.workspace && swimlaneViewDefinition.uri && !swimlaneViewDefinition.get(_Root._SwimlaneViewDefinition.swimlaneConfiguration)) {
            swimlaneViewDefinition = await ClientItems.getObjectByUri(swimlaneViewDefinition.workspace, swimlaneViewDefinition.uri);
        }

        if (swimlaneViewDefinition === undefined) {
            throw new Error("Failed to load SwimlaneViewDefinition");
        }

        let swimlaneConfig = swimlaneViewDefinition.get(_Root._SwimlaneViewDefinition.swimlaneConfiguration, Mof.ObjectType.Object);
        if (swimlaneConfig === undefined) {
            throw new Error("SwimlaneConfiguration is undefined");
        }

        // If swimlaneConfig is a reference without loaded properties, fetch it from server
        if (swimlaneConfig.workspace && swimlaneConfig.uri && !swimlaneConfig.get(_Root._SwimlaneConfiguration.verticalSwimlaneProperty)) {
            swimlaneConfig = await ClientItems.getObjectByUri(swimlaneConfig.workspace, swimlaneConfig.uri);
        }

        if (swimlaneConfig === undefined) {
            throw new Error("Failed to load SwimlaneConfiguration");
        }

        const viewNode = swimlaneViewDefinition.get(_Root._SwimlaneViewDefinition.viewNode, Mof.ObjectType.Object);
        if (viewNode === undefined) {
            throw new Error("ViewNode is undefined");
        }

        const query = new QueryEngine.QueryBuilder();
        QueryEngine.referenceExistingNode(query, viewNode.workspace, viewNode.uri);

        const viewData = await ClientElements.queryObject(query.queryStatement);
        const items = (viewData.result ?? []) as DmObject[];

        const title = configuration.form.get(_Root._SwimlaneForm.title, Mof.ObjectType.String)
            ?? swimlaneViewDefinition.get(_Root._SwimlaneViewDefinition._name_, Mof.ObjectType.String)
            ?? "";

        const swimlaneData = buildSwimlaneData(items, swimlaneConfig, title);
        renderInto(parent, swimlaneData);
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

export function buildSwimlaneData(items: DmObject[], config: DmObject, title?: string): SwimlaneData {
    const verticalProperty = config.get(_Root._SwimlaneConfiguration.verticalSwimlaneProperty, Mof.ObjectType.String) ?? "";
    const horizontalProperty = config.get(_Root._SwimlaneConfiguration.horizontalSwimlaneProperty, Mof.ObjectType.String) ?? "";
    const cellContentTemplate = config.get(_Root._SwimlaneConfiguration.cellContent, Mof.ObjectType.String);
    const linkContent = config.get(_Root._SwimlaneConfiguration.linkContent, Mof.ObjectType.Boolean) ?? false;

    const verticalHeadersSet = new Set<string>();
    const horizontalHeadersSet = new Set<string>();
    const verticalHeaders: string[] = [];
    const horizontalHeaders: string[] = [];

    const cells: { [key: string]: SwimlaneCellItem[] } = {};

    for (const item of items) {
        let vVal = item.get(verticalProperty, Mof.ObjectType.String);
        if (vVal === undefined || vVal === null) {
            const rawV = item.get(verticalProperty);
            vVal = rawV !== undefined && rawV !== null ? String(rawV) : "";
        }

        let hVal = item.get(horizontalProperty, Mof.ObjectType.String);
        if (hVal === undefined || hVal === null) {
            const rawH = item.get(horizontalProperty);
            hVal = rawH !== undefined && rawH !== null ? String(rawH) : "";
        }

        if (!verticalHeadersSet.has(vVal)) {
            verticalHeadersSet.add(vVal);
            verticalHeaders.push(vVal);
        }
        if (!horizontalHeadersSet.has(hVal)) {
            horizontalHeadersSet.add(hVal);
            horizontalHeaders.push(hVal);
        }

        let label: string;
        if (cellContentTemplate !== undefined && cellContentTemplate !== null && cellContentTemplate.trim() !== "") {
            label = renderCellTemplate(cellContentTemplate, item);
        } else {
            label = item.get("name", Mof.ObjectType.String)
                ?? item.get("id", Mof.ObjectType.String)
                ?? (item.id !== undefined && item.id !== "" ? item.id : item.uri)
                ?? "";
        }

        let link: string | undefined = undefined;
        if (linkContent) {
            if (item.workspace && item.uri) {
                link = Navigator.getLinkForNavigateToItemByUrl(item.workspace, item.uri);
            } else if (item.uri) {
                link = item.uri;
            }
        }

        const key = cellKey(hVal, vVal);
        if (!cells[key]) {
            cells[key] = [];
        }
        cells[key].push({ label, link });
    }

    return {
        horizontalHeader: horizontalProperty,
        verticalHeaders,
        horizontalHeaders,
        cells,
        title
    };
}

function renderCellTemplate(template: string, item: DmObject): string {
    if (template.includes("{{")) {
        return template.replace(/\{\{\s*([a-zA-Z0-9_.]+)\s*\}\}/g, (match, propName) => {
            if (propName.startsWith("item.")) {
                propName = propName.substring(5);
            }
            const val = item.get(propName);
            return val !== undefined && val !== null ? String(val) : "";
        });
    }

    const val = item.get(template);
    if (val !== undefined && val !== null) {
        return String(val);
    }
    return template;
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
    export class _SwimlaneForm
    {
        static swimlaneViewDefinition = "swimlaneViewDefinition";
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }

    export const __SwimlaneForm_Uri = "dm:///intern.types.swimlane.datenmeister/#142cb4a2-9a07-4e63-a212-32b0a1f0a289";
}