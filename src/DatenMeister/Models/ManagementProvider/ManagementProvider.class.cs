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

        }

        public _ExtentLoadingState @ExtentLoadingState = new _ExtentLoadingState();
        public IElement @__ExtentLoadingState = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState");

        public class _Extent
        {
            public static string @uri = "uri";
            public IElement? _uri = new MofObjectShadow("dm:///_internal/types/internal#daccfba4-46b3-419c-8166-2d64fd102bf3");

            public static string @count = "count";
            public IElement? _count = new MofObjectShadow("dm:///_internal/types/internal#8399b237-f36f-4a3b-9374-3aecc00e5d1c");

            public static string @totalCount = "totalCount";
            public IElement? _totalCount = new MofObjectShadow("dm:///_internal/types/internal#4b292176-fd82-471f-8a64-2ca3643b6eda");

            public static string @type = "type";
            public IElement? _type = new MofObjectShadow("dm:///_internal/types/internal#1bb7cb0e-337a-4412-9fa2-6b7c60f1db63");

            public static string @extentType = "extentType";
            public IElement? _extentType = new MofObjectShadow("dm:///_internal/types/internal#8048b8e0-9af2-472f-bf65-b9be565501e1");

            public static string @isModified = "isModified";
            public IElement? _isModified = new MofObjectShadow("dm:///_internal/types/internal#4b967a84-b502-4064-ab01-52088b29e36c");

            public static string @alternativeUris = "alternativeUris";
            public IElement? _alternativeUris = new MofObjectShadow("dm:///_internal/types/internal#8deef577-b877-4c29-a664-e555aeeea5a4");

            public static string @state = "state";
            public IElement? _state = new MofObjectShadow("dm:///_internal/types/internal#9fbdca01-a5b3-4d41-890d-7327de330461");

            public static string @failMessage = "failMessage";
            public IElement? _failMessage = new MofObjectShadow("dm:///_internal/types/internal#a824396f-aa69-4a29-ba78-3fe744693c51");

        }

        public _Extent @Extent = new _Extent();
        public IElement @__Extent = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

        public class _Workspace
        {
            public static string @id = "id";
            public IElement? _id = new MofObjectShadow("dm:///_internal/types/internal#c4d09e8d-a97b-431d-add7-10ebdcdcf423");

            public static string @annotation = "annotation";
            public IElement? _annotation = new MofObjectShadow("dm:///_internal/types/internal#5bc9b036-7918-4b83-b074-5a2066bf2f88");

            public static string @extents = "extents";
            public IElement? _extents = new MofObjectShadow("dm:///_internal/types/internal#24a104c9-0298-4833-84e5-91f696ff88ec");

        }

        public _Workspace @Workspace = new _Workspace();
        public IElement @__Workspace = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

        public class _CreateNewWorkspaceModel
        {
            public static string @id = "id";
            public IElement? _id = new MofObjectShadow("dm:///_internal/types/internal#66d29efa-1763-421c-84c2-1d4cdfc936b2");

            public static string @annotation = "annotation";
            public IElement? _annotation = new MofObjectShadow("dm:///_internal/types/internal#8daa3e77-1e0e-45b1-8461-b6f717a9e749");

        }

        public _CreateNewWorkspaceModel @CreateNewWorkspaceModel = new _CreateNewWorkspaceModel();
        public IElement @__CreateNewWorkspaceModel = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

        public class _ExtentTypeSetting
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#8028b070-7663-4ea8-b66b-132e742833ab");

            public static string @rootElementMetaClasses = "rootElementMetaClasses";
            public IElement? _rootElementMetaClasses = new MofObjectShadow("dm:///_internal/types/internal#995b2c16-853b-4c54-8010-00d7f3ffac29");

        }

        public _ExtentTypeSetting @ExtentTypeSetting = new _ExtentTypeSetting();
        public IElement @__ExtentTypeSetting = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

        public class _ExtentProperties
        {
            public static string @uri = "uri";
            public IElement? _uri = new MofObjectShadow("dm:///_internal/types/internal#2bb535c9-6ef1-419e-b7bb-eba470bab97d");

            public static string @count = "count";
            public IElement? _count = new MofObjectShadow("dm:///_internal/types/internal#f26a6c55-0b35-439e-a6f9-39f3b24868a5");

            public static string @totalCount = "totalCount";
            public IElement? _totalCount = new MofObjectShadow("dm:///_internal/types/internal#62c1ebe1-2051-4a16-8dd0-189e6ce25507");

            public static string @type = "type";
            public IElement? _type = new MofObjectShadow("dm:///_internal/types/internal#77822237-9534-4c14-b485-4c7fabe9a283");

            public static string @extentType = "extentType";
            public IElement? _extentType = new MofObjectShadow("dm:///_internal/types/internal#b015b07d-5b3d-48de-9890-70526a172b07");

            public static string @isModified = "isModified";
            public IElement? _isModified = new MofObjectShadow("dm:///_internal/types/internal#589a6d1f-b5fb-4456-bc71-d75c2ce29383");

            public static string @alternativeUris = "alternativeUris";
            public IElement? _alternativeUris = new MofObjectShadow("dm:///_internal/types/internal#3501d8f4-f340-4e8b-9319-8975f19a4e0b");

            public static string @state = "state";
            public IElement? _state = new MofObjectShadow("dm:///_internal/types/internal#7acc0137-5767-45c7-b6b8-e97f716eec6f");

            public static string @failMessage = "failMessage";
            public IElement? _failMessage = new MofObjectShadow("dm:///_internal/types/internal#3ccfa8d1-0612-4ca7-8c9a-ab479594879d");

        }

        public _ExtentProperties @ExtentProperties = new _ExtentProperties();
        public IElement @__ExtentProperties = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

        public class _ExtentPropertyDefinition
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#8e794740-f8e1-45a6-804a-5f56c0326001");

            public static string @title = "title";
            public IElement? _title = new MofObjectShadow("dm:///_internal/types/internal#0ccc5276-1395-4827-9918-faa205572dcf");

            public static string @metaClass = "metaClass";
            public IElement? _metaClass = new MofObjectShadow("dm:///_internal/types/internal#45e22ef3-51ca-43bd-a338-11e3dced53a4");

        }

        public _ExtentPropertyDefinition @ExtentPropertyDefinition = new _ExtentPropertyDefinition();
        public IElement @__ExtentPropertyDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

        public class _ExtentSettings
        {
            public static string @extentTypeSettings = "extentTypeSettings";
            public IElement? _extentTypeSettings = new MofObjectShadow("dm:///_internal/types/internal#38ce5b00-963c-4dbb-b55a-5d70fb5db4ee");

            public static string @propertyDefinitions = "propertyDefinitions";
            public IElement? _propertyDefinitions = new MofObjectShadow("dm:///_internal/types/internal#7c6a532f-ca24-4d3e-a087-6abe5d18a74d");

        }

        public _ExtentSettings @ExtentSettings = new _ExtentSettings();
        public IElement @__ExtentSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

        public static _ManagementProvider TheOne = new _ManagementProvider();

    }

}
