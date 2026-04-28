// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export namespace _IssueMeister
{
        export class _Issue
        {
            static id = "id";
            static _name_ = "name";
            static state = "state";
            static description = "description";
            static iteration = "iteration";
        }

        export const __Issue_Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Issue";
        export module _IssueState
        {
            export const Open = "Open";
            export const InWork = "InWork";
            export const Closed = "Closed";
        }

        export enum ___IssueState
        {
            Open,
            InWork,
            Closed
        }

        export class _Iteration
        {
            static id = "id";
            static _name_ = "name";
            static endDate = "endDate";
        }

        export const __Iteration_Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Iteration";
}

