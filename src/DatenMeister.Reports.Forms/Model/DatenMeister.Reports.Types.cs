#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Reports.Forms.Model
{
    public class _Root
    {
        public class _ReportForm
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @title = "title";
            public IElement? @_title = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

        }

        public _ReportForm @ReportForm = new _ReportForm();
        public MofObjectShadow @__ReportForm = new MofObjectShadow("dm:///_internal/types/internal#89dea433-d199-4889-95eb-7ef30c0b5695");

        public class _RequestReportAction
        {
            public static string @workspace = "workspace";
            public IElement? @_workspace = null;

            public static string @itemUri = "itemUri";
            public IElement? @_itemUri = null;

            public static string @name = "name";
            public IElement? @_name = null;

            public static string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _RequestReportAction @RequestReportAction = new _RequestReportAction();
        public MofObjectShadow @__RequestReportAction = new MofObjectShadow("dm:///_internal/types/internal#a6f3a0e0-c7f6-4b67-b96f-252f3cf1f27c");

        public class _RequestReportResult
        {
            public static string @report = "report";
            public IElement? @_report = null;

        }

        public _RequestReportResult @RequestReportResult = new _RequestReportResult();
        public MofObjectShadow @__RequestReportResult = new MofObjectShadow("dm:///_internal/types/internal#75aaa247-9e5d-4f8c-ad11-8ba43d996684");

        public static readonly _Root TheOne = new _Root();

    }

}