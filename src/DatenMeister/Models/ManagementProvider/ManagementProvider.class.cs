#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.ManagementProviders
{
    public class _ManagementProvider
    {
        public class _ExtentLoadingState
        {
            public static string @Unknown = "Unknown";
            public IElement @__Unknown = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unknown");
            public static string @Unloaded = "Unloaded";
            public IElement @__Unloaded = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unloaded");
            public static string @Loaded = "Loaded";
            public IElement @__Loaded = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Loaded");
            public static string @Failed = "Failed";
            public IElement @__Failed = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Failed");
            public static string @LoadedReadOnly = "LoadedReadOnly";
            public IElement @__LoadedReadOnly = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-LoadedReadOnly");

        }

        public _ExtentLoadingState @ExtentLoadingState = new _ExtentLoadingState();
        public IElement @__ExtentLoadingState = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState");


        public enum ___ExtentLoadingState
        {
            @Unknown,
            @Unloaded,
            @Loaded,
            @Failed,
            @LoadedReadOnly
        }

        public class _Extent
        {
            public static string @uri = "uri";
            public IElement? @_uri = null;

            public static string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static string @count = "count";
            public IElement? @_count = null;

            public static string @totalCount = "totalCount";
            public IElement? @_totalCount = null;

            public static string @type = "type";
            public IElement? @_type = null;

            public static string @extentType = "extentType";
            public IElement? @_extentType = null;

            public static string @isModified = "isModified";
            public IElement? @_isModified = null;

            public static string @alternativeUris = "alternativeUris";
            public IElement? @_alternativeUris = null;

            public static string @autoEnumerateType = "autoEnumerateType";
            public IElement? @_autoEnumerateType = null;

            public static string @state = "state";
            public IElement? @_state = null;

            public static string @failMessage = "failMessage";
            public IElement? @_failMessage = null;

        }

        public _Extent @Extent = new _Extent();
        public IElement @__Extent = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

        public class _Workspace
        {
            public static string @id = "id";
            public IElement? @_id = null;

            public static string @annotation = "annotation";
            public IElement? @_annotation = null;

            public static string @extents = "extents";
            public IElement? @_extents = null;

        }

        public _Workspace @Workspace = new _Workspace();
        public IElement @__Workspace = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

        public class _CreateNewWorkspaceModel
        {
            public static string @id = "id";
            public IElement? @_id = null;

            public static string @annotation = "annotation";
            public IElement? @_annotation = null;

        }

        public _CreateNewWorkspaceModel @CreateNewWorkspaceModel = new _CreateNewWorkspaceModel();
        public IElement @__CreateNewWorkspaceModel = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

        public class _ExtentTypeSetting
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @rootElementMetaClasses = "rootElementMetaClasses";
            public IElement? @_rootElementMetaClasses = null;

        }

        public _ExtentTypeSetting @ExtentTypeSetting = new _ExtentTypeSetting();
        public IElement @__ExtentTypeSetting = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

        public class _ExtentProperties
        {
            public static string @uri = "uri";
            public IElement? @_uri = null;

            public static string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static string @count = "count";
            public IElement? @_count = null;

            public static string @totalCount = "totalCount";
            public IElement? @_totalCount = null;

            public static string @type = "type";
            public IElement? @_type = null;

            public static string @extentType = "extentType";
            public IElement? @_extentType = null;

            public static string @isModified = "isModified";
            public IElement? @_isModified = null;

            public static string @alternativeUris = "alternativeUris";
            public IElement? @_alternativeUris = null;

            public static string @autoEnumerateType = "autoEnumerateType";
            public IElement? @_autoEnumerateType = null;

            public static string @state = "state";
            public IElement? @_state = null;

            public static string @failMessage = "failMessage";
            public IElement? @_failMessage = null;

        }

        public _ExtentProperties @ExtentProperties = new _ExtentProperties();
        public IElement @__ExtentProperties = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

        public class _ExtentPropertyDefinition
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @title = "title";
            public IElement? @_title = null;

            public static string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

        }

        public _ExtentPropertyDefinition @ExtentPropertyDefinition = new _ExtentPropertyDefinition();
        public IElement @__ExtentPropertyDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

        public class _ExtentSettings
        {
            public static string @extentTypeSettings = "extentTypeSettings";
            public IElement? @_extentTypeSettings = null;

            public static string @propertyDefinitions = "propertyDefinitions";
            public IElement? @_propertyDefinitions = null;

        }

        public _ExtentSettings @ExtentSettings = new _ExtentSettings();
        public IElement @__ExtentSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

        public static readonly _ManagementProvider TheOne = new _ManagementProvider();

    }

}
