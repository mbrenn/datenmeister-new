// This file is generated in build.cake. Do NOT modify that file.
// Modify DatenMeister.Reports.Swimlane.Source.ts
// noinspection DuplicatedCode


import * as FormActions from '/js/datenmeister/FormActions.js';
import {IFormNavigation} from "/js/datenmeister/forms/Interfaces";
import {DmObject} from "/js/datenmeister/Mof";
import {SubmitMethod} from "/js/datenmeister/forms/Forms";

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
        alert('Execute Form');
        return Promise.resolve(undefined);
    }
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
        export class _SwimlaneViewdefinition
        {
            static swimlaneConfiguration = "swimlaneConfiguration";
            static viewNode = "viewNode";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __SwimlaneViewdefinition_Uri = "dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c";
}

