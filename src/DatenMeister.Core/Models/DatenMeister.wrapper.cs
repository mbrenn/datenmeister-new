using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.2.0.0
namespace DatenMeister.Core.Models
{
    public class CommonTypes
    {
        public class Default
        {
            public class Package_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @name
                {
                    get =>
                        innerDmElement.getOrDefault<string>("name");
                    set => 
                        innerDmElement.set("name", value);
                }

                public object? @packagedElement
                {
                    get =>
                        innerDmElement.get("packagedElement");
                    set => 
                        innerDmElement.set("packagedElement", value);
                }

                public object? @preferredType
                {
                    get =>
                        innerDmElement.get("preferredType");
                    set => 
                        innerDmElement.set("preferredType", value);
                }

                public object? @preferredPackage
                {
                    get =>
                        innerDmElement.get("preferredPackage");
                    set => 
                        innerDmElement.set("preferredPackage", value);
                }

                public string @defaultViewMode
                {
                    get =>
                        innerDmElement.getOrDefault<string>("defaultViewMode");
                    set => 
                        innerDmElement.set("defaultViewMode", value);
                }

            }

            public class XmiExportContainer_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @xmi
                {
                    get =>
                        innerDmElement.getOrDefault<string>("xmi");
                    set => 
                        innerDmElement.set("xmi", value);
                }

            }

            public class XmiImportContainer_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @xmi
                {
                    get =>
                        innerDmElement.getOrDefault<string>("xmi");
                    set => 
                        innerDmElement.set("xmi", value);
                }

                public string @property
                {
                    get =>
                        innerDmElement.getOrDefault<string>("property");
                    set => 
                        innerDmElement.set("property", value);
                }

                public bool @addToCollection
                {
                    get =>
                        innerDmElement.getOrDefault<bool>("addToCollection");
                    set => 
                        innerDmElement.set("addToCollection", value);
                }

            }

        }

        public class ExtentManager
        {
            public class ImportSettings_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @filePath
                {
                    get =>
                        innerDmElement.getOrDefault<string>("filePath");
                    set => 
                        innerDmElement.set("filePath", value);
                }

                public string @extentUri
                {
                    get =>
                        innerDmElement.getOrDefault<string>("extentUri");
                    set => 
                        innerDmElement.set("extentUri", value);
                }

                public string @workspaceId
                {
                    get =>
                        innerDmElement.getOrDefault<string>("workspaceId");
                    set => 
                        innerDmElement.set("workspaceId", value);
                }

            }

            public class ImportException_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @message
                {
                    get =>
                        innerDmElement.getOrDefault<string>("message");
                    set => 
                        innerDmElement.set("message", value);
                }

            }

        }

    }

    public class Actions
    {
        public class ActionSet_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @action
            {
                get =>
                    innerDmElement.get("action");
                set => 
                    innerDmElement.set("action", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class LoggingWriterAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @message
            {
                get =>
                    innerDmElement.getOrDefault<string>("message");
                set => 
                    innerDmElement.set("message", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class CommandExecutionAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @command
            {
                get =>
                    innerDmElement.getOrDefault<string>("command");
                set => 
                    innerDmElement.set("command", value);
            }

            public string @arguments
            {
                get =>
                    innerDmElement.getOrDefault<string>("arguments");
                set => 
                    innerDmElement.set("arguments", value);
            }

            public string @workingDirectory
            {
                get =>
                    innerDmElement.getOrDefault<string>("workingDirectory");
                set => 
                    innerDmElement.set("workingDirectory", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class PowershellExecutionAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @script
            {
                get =>
                    innerDmElement.getOrDefault<string>("script");
                set => 
                    innerDmElement.set("script", value);
            }

            public string @workingDirectory
            {
                get =>
                    innerDmElement.getOrDefault<string>("workingDirectory");
                set => 
                    innerDmElement.set("workingDirectory", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class LoadExtentAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @configuration
            {
                get =>
                    innerDmElement.get("configuration");
                set => 
                    innerDmElement.set("configuration", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class DropExtentAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class CreateWorkspaceAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @annotation
            {
                get =>
                    innerDmElement.getOrDefault<string>("annotation");
                set => 
                    innerDmElement.set("annotation", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class DropWorkspaceAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class CopyElementsAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @sourcePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("sourcePath");
                set => 
                    innerDmElement.set("sourcePath", value);
            }

            public string @targetPath
            {
                get =>
                    innerDmElement.getOrDefault<string>("targetPath");
                set => 
                    innerDmElement.set("targetPath", value);
            }

            public bool @moveOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("moveOnly");
                set => 
                    innerDmElement.set("moveOnly", value);
            }

            public string @sourceWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("sourceWorkspace");
                set => 
                    innerDmElement.set("sourceWorkspace", value);
            }

            public string @targetWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("targetWorkspace");
                set => 
                    innerDmElement.set("targetWorkspace", value);
            }

            public bool @emptyTarget
            {
                get =>
                    innerDmElement.getOrDefault<bool>("emptyTarget");
                set => 
                    innerDmElement.set("emptyTarget", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class ExportToXmiAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @sourcePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("sourcePath");
                set => 
                    innerDmElement.set("sourcePath", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @sourceWorkspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("sourceWorkspaceId");
                set => 
                    innerDmElement.set("sourceWorkspaceId", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class ClearCollectionAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @path
            {
                get =>
                    innerDmElement.getOrDefault<string>("path");
                set => 
                    innerDmElement.set("path", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class TransformItemsAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public string @runtimeClass
            {
                get =>
                    innerDmElement.getOrDefault<string>("runtimeClass");
                set => 
                    innerDmElement.set("runtimeClass", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @path
            {
                get =>
                    innerDmElement.getOrDefault<string>("path");
                set => 
                    innerDmElement.set("path", value);
            }

            public bool @excludeDescendents
            {
                get =>
                    innerDmElement.getOrDefault<bool>("excludeDescendents");
                set => 
                    innerDmElement.set("excludeDescendents", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class EchoAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @shallSuccess
            {
                get =>
                    innerDmElement.getOrDefault<string>("shallSuccess");
                set => 
                    innerDmElement.set("shallSuccess", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class DocumentOpenAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class CreateFormByMetaClass_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public string @creationMode
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationMode");
                set => 
                    innerDmElement.set("creationMode", value);
            }

            public object? @targetContainer
            {
                get =>
                    innerDmElement.get("targetContainer");
                set => 
                    innerDmElement.set("targetContainer", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class Action_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class MoveOrCopyAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @copyMode
            {
                get =>
                    innerDmElement.getOrDefault<string>("copyMode");
                set => 
                    innerDmElement.set("copyMode", value);
            }

            public object? @target
            {
                get =>
                    innerDmElement.get("target");
                set => 
                    innerDmElement.set("target", value);
            }

            public object? @source
            {
                get =>
                    innerDmElement.get("source");
                set => 
                    innerDmElement.set("source", value);
            }

        }

        public class MoveUpDownAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @element
            {
                get =>
                    innerDmElement.get("element");
                set => 
                    innerDmElement.set("element", value);
            }

            public object? @direction
            {
                get =>
                    innerDmElement.get("direction");
                set => 
                    innerDmElement.set("direction", value);
            }

            public object? @container
            {
                get =>
                    innerDmElement.get("container");
                set => 
                    innerDmElement.set("container", value);
            }

            public string @property
            {
                get =>
                    innerDmElement.getOrDefault<string>("property");
                set => 
                    innerDmElement.set("property", value);
            }

        }

        public class StoreExtentAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class ImportXmiAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @itemUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("itemUri");
                set => 
                    innerDmElement.set("itemUri", value);
            }

            public string @xmi
            {
                get =>
                    innerDmElement.getOrDefault<string>("xmi");
                set => 
                    innerDmElement.set("xmi", value);
            }

            public string @property
            {
                get =>
                    innerDmElement.getOrDefault<string>("property");
                set => 
                    innerDmElement.set("property", value);
            }

            public bool @addToCollection
            {
                get =>
                    innerDmElement.getOrDefault<bool>("addToCollection");
                set => 
                    innerDmElement.set("addToCollection", value);
            }

        }

        public class DeletePropertyFromCollectionAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @propertyName
            {
                get =>
                    innerDmElement.getOrDefault<string>("propertyName");
                set => 
                    innerDmElement.set("propertyName", value);
            }

            public object? @metaclass
            {
                get =>
                    innerDmElement.get("metaclass");
                set => 
                    innerDmElement.set("metaclass", value);
            }

            public string @collectionUrl
            {
                get =>
                    innerDmElement.getOrDefault<string>("collectionUrl");
                set => 
                    innerDmElement.set("collectionUrl", value);
            }

        }

        public class MoveOrCopyActionResult_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @targetUrl
            {
                get =>
                    innerDmElement.getOrDefault<string>("targetUrl");
                set => 
                    innerDmElement.set("targetUrl", value);
            }

            public string @targetWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("targetWorkspace");
                set => 
                    innerDmElement.set("targetWorkspace", value);
            }

        }

        public class ParameterTypes
        {
            public class NavigationDefineActionParameter_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @actionName
                {
                    get =>
                        innerDmElement.getOrDefault<string>("actionName");
                    set => 
                        innerDmElement.set("actionName", value);
                }

                public string @formUrl
                {
                    get =>
                        innerDmElement.getOrDefault<string>("formUrl");
                    set => 
                        innerDmElement.set("formUrl", value);
                }

                public string @metaClassUrl
                {
                    get =>
                        innerDmElement.getOrDefault<string>("metaClassUrl");
                    set => 
                        innerDmElement.set("metaClassUrl", value);
                }

            }

        }

        public class ActionResult_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @isSuccess
            {
                get =>
                    innerDmElement.get("isSuccess");
                set => 
                    innerDmElement.set("isSuccess", value);
            }

            public object? @clientActions
            {
                get =>
                    innerDmElement.get("clientActions");
                set => 
                    innerDmElement.set("clientActions", value);
            }

        }

        public class ClientActions
        {
            public class ClientAction_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @actionName
                {
                    get =>
                        innerDmElement.getOrDefault<string>("actionName");
                    set => 
                        innerDmElement.set("actionName", value);
                }

                public object? @element
                {
                    get =>
                        innerDmElement.get("element");
                    set => 
                        innerDmElement.set("element", value);
                }

                public object? @parameter
                {
                    get =>
                        innerDmElement.get("parameter");
                    set => 
                        innerDmElement.set("parameter", value);
                }

            }

            public class AlertClientAction_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @messageText
                {
                    get =>
                        innerDmElement.getOrDefault<string>("messageText");
                    set => 
                        innerDmElement.set("messageText", value);
                }

                public string @actionName
                {
                    get =>
                        innerDmElement.getOrDefault<string>("actionName");
                    set => 
                        innerDmElement.set("actionName", value);
                }

                public object? @element
                {
                    get =>
                        innerDmElement.get("element");
                    set => 
                        innerDmElement.set("element", value);
                }

                public object? @parameter
                {
                    get =>
                        innerDmElement.get("parameter");
                    set => 
                        innerDmElement.set("parameter", value);
                }

            }

            public class NavigateToExtentClientAction_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @workspaceId
                {
                    get =>
                        innerDmElement.getOrDefault<string>("workspaceId");
                    set => 
                        innerDmElement.set("workspaceId", value);
                }

                public string @extentUri
                {
                    get =>
                        innerDmElement.getOrDefault<string>("extentUri");
                    set => 
                        innerDmElement.set("extentUri", value);
                }

            }

            public class NavigateToItemClientAction_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @workspaceId
                {
                    get =>
                        innerDmElement.getOrDefault<string>("workspaceId");
                    set => 
                        innerDmElement.set("workspaceId", value);
                }

                public string @itemUrl
                {
                    get =>
                        innerDmElement.getOrDefault<string>("itemUrl");
                    set => 
                        innerDmElement.set("itemUrl", value);
                }

                public string @formUri
                {
                    get =>
                        innerDmElement.getOrDefault<string>("formUri");
                    set => 
                        innerDmElement.set("formUri", value);
                }

            }

        }

    }

    public class DataViews
    {
        public class DataView_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @uri
            {
                get =>
                    innerDmElement.getOrDefault<string>("uri");
                set => 
                    innerDmElement.set("uri", value);
            }

            public object? @viewNode
            {
                get =>
                    innerDmElement.get("viewNode");
                set => 
                    innerDmElement.set("viewNode", value);
            }

        }

        public class ViewNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class SelectByExtentNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class FlattenNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @input
            {
                get =>
                    innerDmElement.get("input");
                set => 
                    innerDmElement.set("input", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class FilterByPropertyValueNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @input
            {
                get =>
                    innerDmElement.get("input");
                set => 
                    innerDmElement.set("input", value);
            }

            public string @property
            {
                get =>
                    innerDmElement.getOrDefault<string>("property");
                set => 
                    innerDmElement.set("property", value);
            }

            public string @value
            {
                get =>
                    innerDmElement.getOrDefault<string>("value");
                set => 
                    innerDmElement.set("value", value);
            }

            public object? @comparisonMode
            {
                get =>
                    innerDmElement.get("comparisonMode");
                set => 
                    innerDmElement.set("comparisonMode", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class FilterByMetaclassNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @input
            {
                get =>
                    innerDmElement.get("input");
                set => 
                    innerDmElement.set("input", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public bool @includeInherits
            {
                get =>
                    innerDmElement.getOrDefault<bool>("includeInherits");
                set => 
                    innerDmElement.set("includeInherits", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class SelectByFullNameNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @input
            {
                get =>
                    innerDmElement.get("input");
                set => 
                    innerDmElement.set("input", value);
            }

            public string @path
            {
                get =>
                    innerDmElement.getOrDefault<string>("path");
                set => 
                    innerDmElement.set("path", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class DynamicSourceNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @nodeName
            {
                get =>
                    innerDmElement.getOrDefault<string>("nodeName");
                set => 
                    innerDmElement.set("nodeName", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class SelectByPathNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @path
            {
                get =>
                    innerDmElement.getOrDefault<string>("path");
                set => 
                    innerDmElement.set("path", value);
            }

        }

        public class QueryStatement_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @nodes
            {
                get =>
                    innerDmElement.get("nodes");
                set => 
                    innerDmElement.set("nodes", value);
            }

            public object? @resultNode
            {
                get =>
                    innerDmElement.get("resultNode");
                set => 
                    innerDmElement.set("resultNode", value);
            }

        }

        public class SelectFromAllWorkspacesNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class SelectByWorkspaceNode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

    }

    public class Reports
    {
        public class ReportDefinition_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public object? @elements
            {
                get =>
                    innerDmElement.get("elements");
                set => 
                    innerDmElement.set("elements", value);
            }

        }

        public class ReportInstanceSource_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @path
            {
                get =>
                    innerDmElement.getOrDefault<string>("path");
                set => 
                    innerDmElement.set("path", value);
            }

        }

        public class ReportInstance_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @reportDefinition
            {
                get =>
                    innerDmElement.get("reportDefinition");
                set => 
                    innerDmElement.set("reportDefinition", value);
            }

            public object? @sources
            {
                get =>
                    innerDmElement.get("sources");
                set => 
                    innerDmElement.set("sources", value);
            }

        }

        public class AdocReportInstance_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @reportDefinition
            {
                get =>
                    innerDmElement.get("reportDefinition");
                set => 
                    innerDmElement.set("reportDefinition", value);
            }

            public object? @sources
            {
                get =>
                    innerDmElement.get("sources");
                set => 
                    innerDmElement.set("sources", value);
            }

        }

        public class HtmlReportInstance_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @cssFile
            {
                get =>
                    innerDmElement.getOrDefault<string>("cssFile");
                set => 
                    innerDmElement.set("cssFile", value);
            }

            public string @cssStyleSheet
            {
                get =>
                    innerDmElement.getOrDefault<string>("cssStyleSheet");
                set => 
                    innerDmElement.set("cssStyleSheet", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @reportDefinition
            {
                get =>
                    innerDmElement.get("reportDefinition");
                set => 
                    innerDmElement.set("reportDefinition", value);
            }

            public object? @sources
            {
                get =>
                    innerDmElement.get("sources");
                set => 
                    innerDmElement.set("sources", value);
            }

        }

        public class SimpleReportConfiguration_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @showDescendents
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showDescendents");
                set => 
                    innerDmElement.set("showDescendents", value);
            }

            public string @rootElement
            {
                get =>
                    innerDmElement.getOrDefault<string>("rootElement");
                set => 
                    innerDmElement.set("rootElement", value);
            }

            public bool @showRootElement
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showRootElement");
                set => 
                    innerDmElement.set("showRootElement", value);
            }

            public bool @showMetaClasses
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showMetaClasses");
                set => 
                    innerDmElement.set("showMetaClasses", value);
            }

            public bool @showFullName
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showFullName");
                set => 
                    innerDmElement.set("showFullName", value);
            }

            public object? @form
            {
                get =>
                    innerDmElement.get("form");
                set => 
                    innerDmElement.set("form", value);
            }

            public object? @descendentMode
            {
                get =>
                    innerDmElement.get("descendentMode");
                set => 
                    innerDmElement.set("descendentMode", value);
            }

            public object? @typeMode
            {
                get =>
                    innerDmElement.get("typeMode");
                set => 
                    innerDmElement.set("typeMode", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

        }

    }

    public class ExtentLoaderConfigs
    {
        public class ExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExcelLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @fixRowCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixRowCount");
                set => 
                    innerDmElement.set("fixRowCount", value);
            }

            public bool @fixColumnCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixColumnCount");
                set => 
                    innerDmElement.set("fixColumnCount", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @sheetName
            {
                get =>
                    innerDmElement.getOrDefault<string>("sheetName");
                set => 
                    innerDmElement.set("sheetName", value);
            }

            public int @offsetRow
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetRow");
                set => 
                    innerDmElement.set("offsetRow", value);
            }

            public int @offsetColumn
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetColumn");
                set => 
                    innerDmElement.set("offsetColumn", value);
            }

            public int @countRows
            {
                get =>
                    innerDmElement.getOrDefault<int>("countRows");
                set => 
                    innerDmElement.set("countRows", value);
            }

            public int @countColumns
            {
                get =>
                    innerDmElement.getOrDefault<int>("countColumns");
                set => 
                    innerDmElement.set("countColumns", value);
            }

            public bool @hasHeader
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hasHeader");
                set => 
                    innerDmElement.set("hasHeader", value);
            }

            public bool @tryMergedHeaderCells
            {
                get =>
                    innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
                set => 
                    innerDmElement.set("tryMergedHeaderCells", value);
            }

            public bool @onlySetColumns
            {
                get =>
                    innerDmElement.getOrDefault<bool>("onlySetColumns");
                set => 
                    innerDmElement.set("onlySetColumns", value);
            }

            public string @idColumnName
            {
                get =>
                    innerDmElement.getOrDefault<string>("idColumnName");
                set => 
                    innerDmElement.set("idColumnName", value);
            }

            public int @skipEmptyRowsCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
                set => 
                    innerDmElement.set("skipEmptyRowsCount", value);
            }

            public object? @columns
            {
                get =>
                    innerDmElement.get("columns");
                set => 
                    innerDmElement.set("columns", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExcelReferenceLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @fixRowCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixRowCount");
                set => 
                    innerDmElement.set("fixRowCount", value);
            }

            public bool @fixColumnCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixColumnCount");
                set => 
                    innerDmElement.set("fixColumnCount", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @sheetName
            {
                get =>
                    innerDmElement.getOrDefault<string>("sheetName");
                set => 
                    innerDmElement.set("sheetName", value);
            }

            public int @offsetRow
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetRow");
                set => 
                    innerDmElement.set("offsetRow", value);
            }

            public int @offsetColumn
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetColumn");
                set => 
                    innerDmElement.set("offsetColumn", value);
            }

            public int @countRows
            {
                get =>
                    innerDmElement.getOrDefault<int>("countRows");
                set => 
                    innerDmElement.set("countRows", value);
            }

            public int @countColumns
            {
                get =>
                    innerDmElement.getOrDefault<int>("countColumns");
                set => 
                    innerDmElement.set("countColumns", value);
            }

            public bool @hasHeader
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hasHeader");
                set => 
                    innerDmElement.set("hasHeader", value);
            }

            public bool @tryMergedHeaderCells
            {
                get =>
                    innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
                set => 
                    innerDmElement.set("tryMergedHeaderCells", value);
            }

            public bool @onlySetColumns
            {
                get =>
                    innerDmElement.getOrDefault<bool>("onlySetColumns");
                set => 
                    innerDmElement.set("onlySetColumns", value);
            }

            public string @idColumnName
            {
                get =>
                    innerDmElement.getOrDefault<string>("idColumnName");
                set => 
                    innerDmElement.set("idColumnName", value);
            }

            public int @skipEmptyRowsCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
                set => 
                    innerDmElement.set("skipEmptyRowsCount", value);
            }

            public object? @columns
            {
                get =>
                    innerDmElement.get("columns");
                set => 
                    innerDmElement.set("columns", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExcelImportLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @extentPath
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentPath");
                set => 
                    innerDmElement.set("extentPath", value);
            }

            public bool @fixRowCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixRowCount");
                set => 
                    innerDmElement.set("fixRowCount", value);
            }

            public bool @fixColumnCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixColumnCount");
                set => 
                    innerDmElement.set("fixColumnCount", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @sheetName
            {
                get =>
                    innerDmElement.getOrDefault<string>("sheetName");
                set => 
                    innerDmElement.set("sheetName", value);
            }

            public int @offsetRow
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetRow");
                set => 
                    innerDmElement.set("offsetRow", value);
            }

            public int @offsetColumn
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetColumn");
                set => 
                    innerDmElement.set("offsetColumn", value);
            }

            public int @countRows
            {
                get =>
                    innerDmElement.getOrDefault<int>("countRows");
                set => 
                    innerDmElement.set("countRows", value);
            }

            public int @countColumns
            {
                get =>
                    innerDmElement.getOrDefault<int>("countColumns");
                set => 
                    innerDmElement.set("countColumns", value);
            }

            public bool @hasHeader
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hasHeader");
                set => 
                    innerDmElement.set("hasHeader", value);
            }

            public bool @tryMergedHeaderCells
            {
                get =>
                    innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
                set => 
                    innerDmElement.set("tryMergedHeaderCells", value);
            }

            public bool @onlySetColumns
            {
                get =>
                    innerDmElement.getOrDefault<bool>("onlySetColumns");
                set => 
                    innerDmElement.set("onlySetColumns", value);
            }

            public string @idColumnName
            {
                get =>
                    innerDmElement.getOrDefault<string>("idColumnName");
                set => 
                    innerDmElement.set("idColumnName", value);
            }

            public int @skipEmptyRowsCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
                set => 
                    innerDmElement.set("skipEmptyRowsCount", value);
            }

            public object? @columns
            {
                get =>
                    innerDmElement.get("columns");
                set => 
                    innerDmElement.set("columns", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExcelExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @idColumnName
            {
                get =>
                    innerDmElement.getOrDefault<string>("idColumnName");
                set => 
                    innerDmElement.set("idColumnName", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class InMemoryLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isLinkedList
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isLinkedList");
                set => 
                    innerDmElement.set("isLinkedList", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class XmlReferenceLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public bool @keepNamespaces
            {
                get =>
                    innerDmElement.getOrDefault<bool>("keepNamespaces");
                set => 
                    innerDmElement.set("keepNamespaces", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExtentFileLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class XmiStorageLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class CsvExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @settings
            {
                get =>
                    innerDmElement.get("settings");
                set => 
                    innerDmElement.set("settings", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class CsvSettings_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @encoding
            {
                get =>
                    innerDmElement.getOrDefault<string>("encoding");
                set => 
                    innerDmElement.set("encoding", value);
            }

            public bool @hasHeader
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hasHeader");
                set => 
                    innerDmElement.set("hasHeader", value);
            }

            public object? @separator
            {
                get =>
                    innerDmElement.get("separator");
                set => 
                    innerDmElement.set("separator", value);
            }

            public string @columns
            {
                get =>
                    innerDmElement.getOrDefault<string>("columns");
                set => 
                    innerDmElement.set("columns", value);
            }

            public string @metaclassUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("metaclassUri");
                set => 
                    innerDmElement.set("metaclassUri", value);
            }

            public bool @trimCells
            {
                get =>
                    innerDmElement.getOrDefault<bool>("trimCells");
                set => 
                    innerDmElement.set("trimCells", value);
            }

        }

        public class ExcelHierarchicalColumnDefinition_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public string @property
            {
                get =>
                    innerDmElement.getOrDefault<string>("property");
                set => 
                    innerDmElement.set("property", value);
            }

        }

        public class ExcelHierarchicalLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @hierarchicalColumns
            {
                get =>
                    innerDmElement.get("hierarchicalColumns");
                set => 
                    innerDmElement.set("hierarchicalColumns", value);
            }

            public bool @skipElementsForLastLevel
            {
                get =>
                    innerDmElement.getOrDefault<bool>("skipElementsForLastLevel");
                set => 
                    innerDmElement.set("skipElementsForLastLevel", value);
            }

            public bool @fixRowCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixRowCount");
                set => 
                    innerDmElement.set("fixRowCount", value);
            }

            public bool @fixColumnCount
            {
                get =>
                    innerDmElement.getOrDefault<bool>("fixColumnCount");
                set => 
                    innerDmElement.set("fixColumnCount", value);
            }

            public string @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string @sheetName
            {
                get =>
                    innerDmElement.getOrDefault<string>("sheetName");
                set => 
                    innerDmElement.set("sheetName", value);
            }

            public int @offsetRow
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetRow");
                set => 
                    innerDmElement.set("offsetRow", value);
            }

            public int @offsetColumn
            {
                get =>
                    innerDmElement.getOrDefault<int>("offsetColumn");
                set => 
                    innerDmElement.set("offsetColumn", value);
            }

            public int @countRows
            {
                get =>
                    innerDmElement.getOrDefault<int>("countRows");
                set => 
                    innerDmElement.set("countRows", value);
            }

            public int @countColumns
            {
                get =>
                    innerDmElement.getOrDefault<int>("countColumns");
                set => 
                    innerDmElement.set("countColumns", value);
            }

            public bool @hasHeader
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hasHeader");
                set => 
                    innerDmElement.set("hasHeader", value);
            }

            public bool @tryMergedHeaderCells
            {
                get =>
                    innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
                set => 
                    innerDmElement.set("tryMergedHeaderCells", value);
            }

            public bool @onlySetColumns
            {
                get =>
                    innerDmElement.getOrDefault<bool>("onlySetColumns");
                set => 
                    innerDmElement.set("onlySetColumns", value);
            }

            public string @idColumnName
            {
                get =>
                    innerDmElement.getOrDefault<string>("idColumnName");
                set => 
                    innerDmElement.set("idColumnName", value);
            }

            public int @skipEmptyRowsCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
                set => 
                    innerDmElement.set("skipEmptyRowsCount", value);
            }

            public object? @columns
            {
                get =>
                    innerDmElement.get("columns");
                set => 
                    innerDmElement.set("columns", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

        public class ExcelColumn_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @header
            {
                get =>
                    innerDmElement.getOrDefault<string>("header");
                set => 
                    innerDmElement.set("header", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class EnvironmentalVariableLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

    }

    public class Forms
    {
        public class FieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class SortingOrder_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDescending
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDescending");
                set => 
                    innerDmElement.set("isDescending", value);
            }

        }

        public class AnyDataFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class CheckboxFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public int @lineHeight
            {
                get =>
                    innerDmElement.getOrDefault<int>("lineHeight");
                set => 
                    innerDmElement.set("lineHeight", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class ActionFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @actionName
            {
                get =>
                    innerDmElement.getOrDefault<string>("actionName");
                set => 
                    innerDmElement.set("actionName", value);
            }

            public object? @parameter
            {
                get =>
                    innerDmElement.get("parameter");
                set => 
                    innerDmElement.set("parameter", value);
            }

            public string @buttonText
            {
                get =>
                    innerDmElement.getOrDefault<string>("buttonText");
                set => 
                    innerDmElement.set("buttonText", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class DateTimeFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @hideDate
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideDate");
                set => 
                    innerDmElement.set("hideDate", value);
            }

            public bool @hideTime
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideTime");
                set => 
                    innerDmElement.set("hideTime", value);
            }

            public bool @showOffsetButtons
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showOffsetButtons");
                set => 
                    innerDmElement.set("showOffsetButtons", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class FormAssociation_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @formType
            {
                get =>
                    innerDmElement.get("formType");
                set => 
                    innerDmElement.set("formType", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public string @extentType
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentType");
                set => 
                    innerDmElement.set("extentType", value);
            }

            public string @viewModeId
            {
                get =>
                    innerDmElement.getOrDefault<string>("viewModeId");
                set => 
                    innerDmElement.set("viewModeId", value);
            }

            public object? @parentMetaClass
            {
                get =>
                    innerDmElement.get("parentMetaClass");
                set => 
                    innerDmElement.set("parentMetaClass", value);
            }

            public string @parentProperty
            {
                get =>
                    innerDmElement.getOrDefault<string>("parentProperty");
                set => 
                    innerDmElement.set("parentProperty", value);
            }

            public object? @form
            {
                get =>
                    innerDmElement.get("form");
                set => 
                    innerDmElement.set("form", value);
            }

            public bool @debugActive
            {
                get =>
                    innerDmElement.getOrDefault<bool>("debugActive");
                set => 
                    innerDmElement.set("debugActive", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

        }

        public class DropDownFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @values
            {
                get =>
                    innerDmElement.get("values");
                set => 
                    innerDmElement.set("values", value);
            }

            public object? @valuesByEnumeration
            {
                get =>
                    innerDmElement.get("valuesByEnumeration");
                set => 
                    innerDmElement.set("valuesByEnumeration", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class ValuePair_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @value
            {
                get =>
                    innerDmElement.get("value");
                set => 
                    innerDmElement.set("value", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        public class MetaClassElementFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class ReferenceFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isSelectionInline
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isSelectionInline");
                set => 
                    innerDmElement.set("isSelectionInline", value);
            }

            public string @defaultWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultWorkspace");
                set => 
                    innerDmElement.set("defaultWorkspace", value);
            }

            public string @defaultItemUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultItemUri");
                set => 
                    innerDmElement.set("defaultItemUri", value);
            }

            public bool @showAllChildren
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showAllChildren");
                set => 
                    innerDmElement.set("showAllChildren", value);
            }

            public bool @showWorkspaceSelection
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showWorkspaceSelection");
                set => 
                    innerDmElement.set("showWorkspaceSelection", value);
            }

            public bool @showExtentSelection
            {
                get =>
                    innerDmElement.getOrDefault<bool>("showExtentSelection");
                set => 
                    innerDmElement.set("showExtentSelection", value);
            }

            public object? @metaClassFilter
            {
                get =>
                    innerDmElement.get("metaClassFilter");
                set => 
                    innerDmElement.set("metaClassFilter", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class SubElementFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public object? @form
            {
                get =>
                    innerDmElement.get("form");
                set => 
                    innerDmElement.set("form", value);
            }

            public bool @allowOnlyExistingElements
            {
                get =>
                    innerDmElement.getOrDefault<bool>("allowOnlyExistingElements");
                set => 
                    innerDmElement.set("allowOnlyExistingElements", value);
            }

            public object? @defaultTypesForNewElements
            {
                get =>
                    innerDmElement.get("defaultTypesForNewElements");
                set => 
                    innerDmElement.set("defaultTypesForNewElements", value);
            }

            public bool @includeSpecializationsForDefaultTypes
            {
                get =>
                    innerDmElement.getOrDefault<bool>("includeSpecializationsForDefaultTypes");
                set => 
                    innerDmElement.set("includeSpecializationsForDefaultTypes", value);
            }

            public string @defaultWorkspaceOfNewElements
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultWorkspaceOfNewElements");
                set => 
                    innerDmElement.set("defaultWorkspaceOfNewElements", value);
            }

            public string @defaultExtentOfNewElements
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultExtentOfNewElements");
                set => 
                    innerDmElement.set("defaultExtentOfNewElements", value);
            }

            public string @actionName
            {
                get =>
                    innerDmElement.getOrDefault<string>("actionName");
                set => 
                    innerDmElement.set("actionName", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class TextFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public int @lineHeight
            {
                get =>
                    innerDmElement.getOrDefault<int>("lineHeight");
                set => 
                    innerDmElement.set("lineHeight", value);
            }

            public int @width
            {
                get =>
                    innerDmElement.getOrDefault<int>("width");
                set => 
                    innerDmElement.set("width", value);
            }

            public int @shortenTextLength
            {
                get =>
                    innerDmElement.getOrDefault<int>("shortenTextLength");
                set => 
                    innerDmElement.set("shortenTextLength", value);
            }

            public bool @supportClipboardCopy
            {
                get =>
                    innerDmElement.getOrDefault<bool>("supportClipboardCopy");
                set => 
                    innerDmElement.set("supportClipboardCopy", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class EvalTextFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @evalCellProperties
            {
                get =>
                    innerDmElement.getOrDefault<string>("evalCellProperties");
                set => 
                    innerDmElement.set("evalCellProperties", value);
            }

            public int @lineHeight
            {
                get =>
                    innerDmElement.getOrDefault<int>("lineHeight");
                set => 
                    innerDmElement.set("lineHeight", value);
            }

            public int @width
            {
                get =>
                    innerDmElement.getOrDefault<int>("width");
                set => 
                    innerDmElement.set("width", value);
            }

            public int @shortenTextLength
            {
                get =>
                    innerDmElement.getOrDefault<int>("shortenTextLength");
                set => 
                    innerDmElement.set("shortenTextLength", value);
            }

            public bool @supportClipboardCopy
            {
                get =>
                    innerDmElement.getOrDefault<bool>("supportClipboardCopy");
                set => 
                    innerDmElement.set("supportClipboardCopy", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class SeparatorLineFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public int @Height
            {
                get =>
                    innerDmElement.getOrDefault<int>("Height");
                set => 
                    innerDmElement.set("Height", value);
            }

        }

        public class FileSelectionFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @defaultExtension
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultExtension");
                set => 
                    innerDmElement.set("defaultExtension", value);
            }

            public bool @isSaving
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isSaving");
                set => 
                    innerDmElement.set("isSaving", value);
            }

            public string @initialPathToDirectory
            {
                get =>
                    innerDmElement.getOrDefault<string>("initialPathToDirectory");
                set => 
                    innerDmElement.set("initialPathToDirectory", value);
            }

            public string @filter
            {
                get =>
                    innerDmElement.getOrDefault<string>("filter");
                set => 
                    innerDmElement.set("filter", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class DefaultTypeForNewElement_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public string @parentProperty
            {
                get =>
                    innerDmElement.getOrDefault<string>("parentProperty");
                set => 
                    innerDmElement.set("parentProperty", value);
            }

        }

        public class FullNameFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class CheckboxListTaggingFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @values
            {
                get =>
                    innerDmElement.get("values");
                set => 
                    innerDmElement.set("values", value);
            }

            public string @separator
            {
                get =>
                    innerDmElement.getOrDefault<string>("separator");
                set => 
                    innerDmElement.set("separator", value);
            }

            public bool @containsFreeText
            {
                get =>
                    innerDmElement.getOrDefault<bool>("containsFreeText");
                set => 
                    innerDmElement.set("containsFreeText", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class NumberFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @format
            {
                get =>
                    innerDmElement.getOrDefault<string>("format");
                set => 
                    innerDmElement.set("format", value);
            }

            public bool @isInteger
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isInteger");
                set => 
                    innerDmElement.set("isInteger", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class Form_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

            public bool @isAutoGenerated
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAutoGenerated");
                set => 
                    innerDmElement.set("isAutoGenerated", value);
            }

            public bool @hideMetaInformation
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideMetaInformation");
                set => 
                    innerDmElement.set("hideMetaInformation", value);
            }

            public string @originalUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalUri");
                set => 
                    innerDmElement.set("originalUri", value);
            }

            public string @originalWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalWorkspace");
                set => 
                    innerDmElement.set("originalWorkspace", value);
            }

            public string @creationProtocol
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationProtocol");
                set => 
                    innerDmElement.set("creationProtocol", value);
            }

        }

        public class RowForm_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @buttonApplyText
            {
                get =>
                    innerDmElement.getOrDefault<string>("buttonApplyText");
                set => 
                    innerDmElement.set("buttonApplyText", value);
            }

            public bool @allowNewProperties
            {
                get =>
                    innerDmElement.getOrDefault<bool>("allowNewProperties");
                set => 
                    innerDmElement.set("allowNewProperties", value);
            }

            public int @defaultWidth
            {
                get =>
                    innerDmElement.getOrDefault<int>("defaultWidth");
                set => 
                    innerDmElement.set("defaultWidth", value);
            }

            public int @defaultHeight
            {
                get =>
                    innerDmElement.getOrDefault<int>("defaultHeight");
                set => 
                    innerDmElement.set("defaultHeight", value);
            }

            public object? @field
            {
                get =>
                    innerDmElement.get("field");
                set => 
                    innerDmElement.set("field", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

            public bool @isAutoGenerated
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAutoGenerated");
                set => 
                    innerDmElement.set("isAutoGenerated", value);
            }

            public bool @hideMetaInformation
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideMetaInformation");
                set => 
                    innerDmElement.set("hideMetaInformation", value);
            }

            public string @originalUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalUri");
                set => 
                    innerDmElement.set("originalUri", value);
            }

            public string @originalWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalWorkspace");
                set => 
                    innerDmElement.set("originalWorkspace", value);
            }

            public string @creationProtocol
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationProtocol");
                set => 
                    innerDmElement.set("creationProtocol", value);
            }

        }

        public class TableForm_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @property
            {
                get =>
                    innerDmElement.getOrDefault<string>("property");
                set => 
                    innerDmElement.set("property", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

            public bool @includeDescendents
            {
                get =>
                    innerDmElement.getOrDefault<bool>("includeDescendents");
                set => 
                    innerDmElement.set("includeDescendents", value);
            }

            public bool @noItemsWithMetaClass
            {
                get =>
                    innerDmElement.getOrDefault<bool>("noItemsWithMetaClass");
                set => 
                    innerDmElement.set("noItemsWithMetaClass", value);
            }

            public bool @inhibitNewItems
            {
                get =>
                    innerDmElement.getOrDefault<bool>("inhibitNewItems");
                set => 
                    innerDmElement.set("inhibitNewItems", value);
            }

            public bool @inhibitDeleteItems
            {
                get =>
                    innerDmElement.getOrDefault<bool>("inhibitDeleteItems");
                set => 
                    innerDmElement.set("inhibitDeleteItems", value);
            }

            public bool @inhibitEditItems
            {
                get =>
                    innerDmElement.getOrDefault<bool>("inhibitEditItems");
                set => 
                    innerDmElement.set("inhibitEditItems", value);
            }

            public object? @defaultTypesForNewElements
            {
                get =>
                    innerDmElement.get("defaultTypesForNewElements");
                set => 
                    innerDmElement.set("defaultTypesForNewElements", value);
            }

            public object? @fastViewFilters
            {
                get =>
                    innerDmElement.get("fastViewFilters");
                set => 
                    innerDmElement.set("fastViewFilters", value);
            }

            public object? @field
            {
                get =>
                    innerDmElement.get("field");
                set => 
                    innerDmElement.set("field", value);
            }

            public object? @sortingOrder
            {
                get =>
                    innerDmElement.get("sortingOrder");
                set => 
                    innerDmElement.set("sortingOrder", value);
            }

            public object? @viewNode
            {
                get =>
                    innerDmElement.get("viewNode");
                set => 
                    innerDmElement.set("viewNode", value);
            }

            public bool @autoGenerateFields
            {
                get =>
                    innerDmElement.getOrDefault<bool>("autoGenerateFields");
                set => 
                    innerDmElement.set("autoGenerateFields", value);
            }

            public bool @duplicatePerType
            {
                get =>
                    innerDmElement.getOrDefault<bool>("duplicatePerType");
                set => 
                    innerDmElement.set("duplicatePerType", value);
            }

            public string @dataUrl
            {
                get =>
                    innerDmElement.getOrDefault<string>("dataUrl");
                set => 
                    innerDmElement.set("dataUrl", value);
            }

            public bool @inhibitNewUnclassifiedItems
            {
                get =>
                    innerDmElement.getOrDefault<bool>("inhibitNewUnclassifiedItems");
                set => 
                    innerDmElement.set("inhibitNewUnclassifiedItems", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

            public bool @isAutoGenerated
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAutoGenerated");
                set => 
                    innerDmElement.set("isAutoGenerated", value);
            }

            public bool @hideMetaInformation
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideMetaInformation");
                set => 
                    innerDmElement.set("hideMetaInformation", value);
            }

            public string @originalUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalUri");
                set => 
                    innerDmElement.set("originalUri", value);
            }

            public string @originalWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalWorkspace");
                set => 
                    innerDmElement.set("originalWorkspace", value);
            }

            public string @creationProtocol
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationProtocol");
                set => 
                    innerDmElement.set("creationProtocol", value);
            }

        }

        public class CollectionForm_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @tab
            {
                get =>
                    innerDmElement.get("tab");
                set => 
                    innerDmElement.set("tab", value);
            }

            public bool @autoTabs
            {
                get =>
                    innerDmElement.getOrDefault<bool>("autoTabs");
                set => 
                    innerDmElement.set("autoTabs", value);
            }

            public object? @field
            {
                get =>
                    innerDmElement.get("field");
                set => 
                    innerDmElement.set("field", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

            public bool @isAutoGenerated
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAutoGenerated");
                set => 
                    innerDmElement.set("isAutoGenerated", value);
            }

            public bool @hideMetaInformation
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideMetaInformation");
                set => 
                    innerDmElement.set("hideMetaInformation", value);
            }

            public string @originalUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalUri");
                set => 
                    innerDmElement.set("originalUri", value);
            }

            public string @originalWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalWorkspace");
                set => 
                    innerDmElement.set("originalWorkspace", value);
            }

            public string @creationProtocol
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationProtocol");
                set => 
                    innerDmElement.set("creationProtocol", value);
            }

        }

        public class ObjectForm_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @tab
            {
                get =>
                    innerDmElement.get("tab");
                set => 
                    innerDmElement.set("tab", value);
            }

            public bool @autoTabs
            {
                get =>
                    innerDmElement.getOrDefault<bool>("autoTabs");
                set => 
                    innerDmElement.set("autoTabs", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

            public bool @isAutoGenerated
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAutoGenerated");
                set => 
                    innerDmElement.set("isAutoGenerated", value);
            }

            public bool @hideMetaInformation
            {
                get =>
                    innerDmElement.getOrDefault<bool>("hideMetaInformation");
                set => 
                    innerDmElement.set("hideMetaInformation", value);
            }

            public string @originalUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalUri");
                set => 
                    innerDmElement.set("originalUri", value);
            }

            public string @originalWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("originalWorkspace");
                set => 
                    innerDmElement.set("originalWorkspace", value);
            }

            public string @creationProtocol
            {
                get =>
                    innerDmElement.getOrDefault<string>("creationProtocol");
                set => 
                    innerDmElement.set("creationProtocol", value);
            }

        }

        public class ViewMode_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @id
            {
                get =>
                    innerDmElement.getOrDefault<string>("id");
                set => 
                    innerDmElement.set("id", value);
            }

            public string @defaultExtentType
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultExtentType");
                set => 
                    innerDmElement.set("defaultExtentType", value);
            }

        }

        public class DropDownByCollection_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @defaultWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("defaultWorkspace");
                set => 
                    innerDmElement.set("defaultWorkspace", value);
            }

            public string @collection
            {
                get =>
                    innerDmElement.getOrDefault<string>("collection");
                set => 
                    innerDmElement.set("collection", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

        public class UriReferenceFieldData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @defaultWorkspace
            {
                get =>
                    innerDmElement.get("defaultWorkspace");
                set => 
                    innerDmElement.set("defaultWorkspace", value);
            }

            public object? @defaultExtent
            {
                get =>
                    innerDmElement.get("defaultExtent");
                set => 
                    innerDmElement.set("defaultExtent", value);
            }

        }

        public class NavigateToFieldsForTestAction_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        public class DropDownByQueryData_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @query
            {
                get =>
                    innerDmElement.get("query");
                set => 
                    innerDmElement.set("query", value);
            }

            public bool @isAttached
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isAttached");
                set => 
                    innerDmElement.set("isAttached", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public bool @isEnumeration
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isEnumeration");
                set => 
                    innerDmElement.set("isEnumeration", value);
            }

            public object? @defaultValue
            {
                get =>
                    innerDmElement.get("defaultValue");
                set => 
                    innerDmElement.set("defaultValue", value);
            }

            public bool @isReadOnly
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isReadOnly");
                set => 
                    innerDmElement.set("isReadOnly", value);
            }

        }

    }

    public class AttachedExtent
    {
        public class AttachedExtentConfiguration_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @referencedWorkspace
            {
                get =>
                    innerDmElement.getOrDefault<string>("referencedWorkspace");
                set => 
                    innerDmElement.set("referencedWorkspace", value);
            }

            public string @referencedExtent
            {
                get =>
                    innerDmElement.getOrDefault<string>("referencedExtent");
                set => 
                    innerDmElement.set("referencedExtent", value);
            }

            public object? @referenceType
            {
                get =>
                    innerDmElement.get("referenceType");
                set => 
                    innerDmElement.set("referenceType", value);
            }

            public string @referenceProperty
            {
                get =>
                    innerDmElement.getOrDefault<string>("referenceProperty");
                set => 
                    innerDmElement.set("referenceProperty", value);
            }

        }

    }

    public class Management
    {
        public class Extent_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @uri
            {
                get =>
                    innerDmElement.getOrDefault<string>("uri");
                set => 
                    innerDmElement.set("uri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public int @count
            {
                get =>
                    innerDmElement.getOrDefault<int>("count");
                set => 
                    innerDmElement.set("count", value);
            }

            public int @totalCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("totalCount");
                set => 
                    innerDmElement.set("totalCount", value);
            }

            public string @type
            {
                get =>
                    innerDmElement.getOrDefault<string>("type");
                set => 
                    innerDmElement.set("type", value);
            }

            public string @extentType
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentType");
                set => 
                    innerDmElement.set("extentType", value);
            }

            public bool @isModified
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isModified");
                set => 
                    innerDmElement.set("isModified", value);
            }

            public string @alternativeUris
            {
                get =>
                    innerDmElement.getOrDefault<string>("alternativeUris");
                set => 
                    innerDmElement.set("alternativeUris", value);
            }

            public string @autoEnumerateType
            {
                get =>
                    innerDmElement.getOrDefault<string>("autoEnumerateType");
                set => 
                    innerDmElement.set("autoEnumerateType", value);
            }

            public object? @state
            {
                get =>
                    innerDmElement.get("state");
                set => 
                    innerDmElement.set("state", value);
            }

            public string @failMessage
            {
                get =>
                    innerDmElement.getOrDefault<string>("failMessage");
                set => 
                    innerDmElement.set("failMessage", value);
            }

            public object? @properties
            {
                get =>
                    innerDmElement.get("properties");
                set => 
                    innerDmElement.set("properties", value);
            }

            public object? @loadingConfiguration
            {
                get =>
                    innerDmElement.get("loadingConfiguration");
                set => 
                    innerDmElement.set("loadingConfiguration", value);
            }

        }

        public class Workspace_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @id
            {
                get =>
                    innerDmElement.getOrDefault<string>("id");
                set => 
                    innerDmElement.set("id", value);
            }

            public string @annotation
            {
                get =>
                    innerDmElement.getOrDefault<string>("annotation");
                set => 
                    innerDmElement.set("annotation", value);
            }

            public object? @extents
            {
                get =>
                    innerDmElement.get("extents");
                set => 
                    innerDmElement.set("extents", value);
            }

        }

        public class CreateNewWorkspaceModel_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @id
            {
                get =>
                    innerDmElement.getOrDefault<string>("id");
                set => 
                    innerDmElement.set("id", value);
            }

            public string @annotation
            {
                get =>
                    innerDmElement.getOrDefault<string>("annotation");
                set => 
                    innerDmElement.set("annotation", value);
            }

        }

        public class ExtentTypeSetting_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public object? @rootElementMetaClasses
            {
                get =>
                    innerDmElement.get("rootElementMetaClasses");
                set => 
                    innerDmElement.set("rootElementMetaClasses", value);
            }

        }

        public class ExtentProperties_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @uri
            {
                get =>
                    innerDmElement.getOrDefault<string>("uri");
                set => 
                    innerDmElement.set("uri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public int @count
            {
                get =>
                    innerDmElement.getOrDefault<int>("count");
                set => 
                    innerDmElement.set("count", value);
            }

            public int @totalCount
            {
                get =>
                    innerDmElement.getOrDefault<int>("totalCount");
                set => 
                    innerDmElement.set("totalCount", value);
            }

            public string @type
            {
                get =>
                    innerDmElement.getOrDefault<string>("type");
                set => 
                    innerDmElement.set("type", value);
            }

            public string @extentType
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentType");
                set => 
                    innerDmElement.set("extentType", value);
            }

            public bool @isModified
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isModified");
                set => 
                    innerDmElement.set("isModified", value);
            }

            public string @alternativeUris
            {
                get =>
                    innerDmElement.getOrDefault<string>("alternativeUris");
                set => 
                    innerDmElement.set("alternativeUris", value);
            }

            public string @autoEnumerateType
            {
                get =>
                    innerDmElement.getOrDefault<string>("autoEnumerateType");
                set => 
                    innerDmElement.set("autoEnumerateType", value);
            }

            public object? @state
            {
                get =>
                    innerDmElement.get("state");
                set => 
                    innerDmElement.set("state", value);
            }

            public string @failMessage
            {
                get =>
                    innerDmElement.getOrDefault<string>("failMessage");
                set => 
                    innerDmElement.set("failMessage", value);
            }

            public object? @properties
            {
                get =>
                    innerDmElement.get("properties");
                set => 
                    innerDmElement.set("properties", value);
            }

            public object? @loadingConfiguration
            {
                get =>
                    innerDmElement.get("loadingConfiguration");
                set => 
                    innerDmElement.set("loadingConfiguration", value);
            }

        }

        public class ExtentPropertyDefinition_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @title
            {
                get =>
                    innerDmElement.getOrDefault<string>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public object? @metaClass
            {
                get =>
                    innerDmElement.get("metaClass");
                set => 
                    innerDmElement.set("metaClass", value);
            }

        }

        public class ExtentSettings_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public object? @extentTypeSettings
            {
                get =>
                    innerDmElement.get("extentTypeSettings");
                set => 
                    innerDmElement.set("extentTypeSettings", value);
            }

            public object? @propertyDefinitions
            {
                get =>
                    innerDmElement.get("propertyDefinitions");
                set => 
                    innerDmElement.set("propertyDefinitions", value);
            }

        }

    }

    public class FastViewFilters
    {
        public class PropertyComparisonFilter_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @Property
            {
                get =>
                    innerDmElement.getOrDefault<string>("Property");
                set => 
                    innerDmElement.set("Property", value);
            }

            public object? @ComparisonType
            {
                get =>
                    innerDmElement.get("ComparisonType");
                set => 
                    innerDmElement.set("ComparisonType", value);
            }

            public string @Value
            {
                get =>
                    innerDmElement.getOrDefault<string>("Value");
                set => 
                    innerDmElement.set("Value", value);
            }

        }

        public class PropertyContainsFilter_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @Property
            {
                get =>
                    innerDmElement.getOrDefault<string>("Property");
                set => 
                    innerDmElement.set("Property", value);
            }

            public string @Value
            {
                get =>
                    innerDmElement.getOrDefault<string>("Value");
                set => 
                    innerDmElement.set("Value", value);
            }

        }

    }

    public class DynamicRuntimeProvider
    {
        public class DynamicRuntimeLoaderConfig_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @runtimeClass
            {
                get =>
                    innerDmElement.getOrDefault<string>("runtimeClass");
                set => 
                    innerDmElement.set("runtimeClass", value);
            }

            public object? @configuration
            {
                get =>
                    innerDmElement.get("configuration");
                set => 
                    innerDmElement.set("configuration", value);
            }

            public string @name
            {
                get =>
                    innerDmElement.getOrDefault<string>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public bool @dropExisting
            {
                get =>
                    innerDmElement.getOrDefault<bool>("dropExisting");
                set => 
                    innerDmElement.set("dropExisting", value);
            }

        }

    }

    public class Verifier
    {
        public class VerifyEntry_Wrapper(IElement innerDmElement)
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string @itemUri
            {
                get =>
                    innerDmElement.getOrDefault<string>("itemUri");
                set => 
                    innerDmElement.set("itemUri", value);
            }

            public string @category
            {
                get =>
                    innerDmElement.getOrDefault<string>("category");
                set => 
                    innerDmElement.set("category", value);
            }

            public string @message
            {
                get =>
                    innerDmElement.getOrDefault<string>("message");
                set => 
                    innerDmElement.set("message", value);
            }

        }

    }

}
