#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Reports.Forms.Model
{
    public class _Root
    {
        public class _ReportFieldData
        {
            public static string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static string @name = "name";
            public IElement? @_name = null;

            public static string @title = "title";
            public IElement? @_title = null;

            public static string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _ReportFieldData @ReportFieldData = new _ReportFieldData();
        public MofObjectShadow @__ReportFieldData = new MofObjectShadow("dm:///types/#89dea433-d199-4889-95eb-7ef30c0b5695");

        public static readonly _Root TheOne = new _Root();

    }

}
