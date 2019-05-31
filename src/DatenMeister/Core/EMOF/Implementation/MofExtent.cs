﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Provider;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class MofExtent : IExtent, IHasWorkspace
    {
        /// <summary>
        /// This type lookup can be used to convert the instances of the .Net types to real MOF meta classes. 
        /// It is only used, if the data is directly set as a .Net object
        /// </summary>
        public IDotNetTypeLookup TypeLookup { get; }

        /// <summary>
        /// Gets or sets the provider for the given extent
        /// </summary>
        public IProvider Provider { get; }

        /// <summary>
        /// Gets or sets the workspace to which the extent is allocated
        /// </summary>
        protected Workspace _Workspace { get; set; }

        /// <summary>
        /// Gets or sets the workspace to which the extent is allocated
        /// </summary>
        public IWorkspace Workspace
        {
            get => _Workspace;
            set => _Workspace = (Workspace) value;
        }
        
        /// <summary>
        /// Gets the workspace via the interface
        /// </summary>
        IWorkspace IHasWorkspace.Workspace => Workspace;

        /// <summary>
        /// Stores a list of other extents that shall also be considered as meta extents
        /// </summary>
        private readonly List<IUriExtent> _metaExtents = new List<IUriExtent>();

        /// <summary>
        /// Gets or sets the change event manager for the objects within
        /// </summary>
        internal ChangeEventManager ChangeEventManager { get; set; }

        /// <summary>
        /// Gets the meta object representing the meta object. Setting, querying a list or getting
        /// is supported by this object
        /// </summary>
        /// <returns>The returned value representing the meta object</returns>
        public MofObject GetMetaObject()
        {
            if ((Provider.GetCapabilities() & ProviderCapability.StoreMetaDataInExtent) ==
                ProviderCapability.StoreMetaDataInExtent)
            {
                var nullObject = Provider.Get(null) ??
                                 throw new InvalidOperationException(
                                     "Provider does not support setting of extent properties");

                return new MofObject(nullObject, this);
            }
            else
            {
                return MetaXmiElement;
            }
        }

        /// <summary>
        /// Gets the meta element for xmi data
        /// </summary>
        public MofObject MetaXmiElement { get; set; }

        /// <summary>
        /// Gets or sets the xml Node of the meta element.
        /// Only to be used by ExtentConfigurationLoader and other extent loaders.
        /// This method shall only be called, if the underlying provider does not support storage of metadata
        /// </summary>
        public XElement LocalMetaElementXmlNode
        {
            get => ((XmiProviderObject) MetaXmiElement.ProviderObject).XmlNode;
            set => ((XmiProviderObject) MetaXmiElement.ProviderObject).XmlNode = value;
        }

        /// <summary>
        /// Initializes a new instance of the Extent 
        /// </summary>
        /// <param name="provider">Provider being used for the extent</param>
        public MofExtent(IProvider provider, ChangeEventManager changeEventManager = null)
        {
            ChangeEventManager = changeEventManager;

            var xmiProvider = new XmiProvider();
            Provider = provider;
            TypeLookup = new DotNetTypeLookup();
            MetaXmiElement = new MofObject(
                new XmiProviderObject(new XElement("meta"), xmiProvider),
                    this);
        }

        /// <inheritdoc />
        public bool @equals(object other)
        {
            if (other is MofExtent otherAsExtent)
            {
                return Equals(otherAsExtent);
            }

            return false;
        }

        /// <inheritdoc />
        public object get(string property)
        {
            if ((Provider.GetCapabilities() & ProviderCapability.StoreMetaDataInExtent) ==
                ProviderCapability.StoreMetaDataInExtent)
            {
                var nullObject = Provider.Get(null) ??
                                 throw new InvalidOperationException(
                                     "Provider does not support setting of extent properties");
                return nullObject.GetProperty(property);
            }
            else
            {
                return MetaXmiElement.get(property);
            }
        }

        /// <inheritdoc />
        public void set(string property, object value)
        {
            if ((Provider.GetCapabilities() & ProviderCapability.StoreMetaDataInExtent) ==
                ProviderCapability.StoreMetaDataInExtent)
            {
                var nullObject = Provider.Get(null) ??
                                 throw new InvalidOperationException(
                                     "Provider does not support setting of extent properties");
                nullObject.SetProperty(property, value);
            }
            else
            {
                MetaXmiElement.set(property, value);
            }
        }

        /// <inheritdoc />
        public bool isSet(string property)
        {
            if ((Provider.GetCapabilities() & ProviderCapability.StoreMetaDataInExtent) ==
                ProviderCapability.StoreMetaDataInExtent)
            {
                return Provider.Get(null)?.IsPropertySet(property) ?? false;
            }
            else
            {
                return MetaXmiElement.isSet(property);
            }
        }

        /// <inheritdoc />
        public void unset(string property)
        {
            if ((Provider.GetCapabilities() & ProviderCapability.StoreMetaDataInExtent) ==
                ProviderCapability.StoreMetaDataInExtent)
            {
                Provider.Get(null)?.DeleteProperty(property);
            }
            else
            {
                MetaXmiElement.unset(property);
            }
        }

        /// <inheritdoc />
        public bool useContainment()
        {
            return false;
        }

        /// <inheritdoc />
        public IReflectiveSequence elements()
        {
            return new ExtentReflectiveSequence(this);
        }

        /// <summary>
        /// Adds an extent as a meta extent, so it will also be used to retrieve the element
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        public void AddMetaExtent(IUriExtent extent)
        {
            lock (_metaExtents)
            {
                if (_metaExtents.Any(x => x.contextURI() == extent.contextURI()))
                {
                    // Already in 
                    return;
                }

                _metaExtents.Add(extent);
            }
        }

        /// <summary>
        /// Gets the meta extents
        /// </summary>
        public IEnumerable<IUriExtent> MetaExtents
        {
            get
            {
                lock (_metaExtents)
                {
                    return _metaExtents.ToList();
                }
            }
        }

        /// <summary>
        /// Resolves the DotNetType by navigating through the current and the meta instances. 
        /// </summary>
        /// <param name="metaclassUri">Uri class to be retrieved</param>
        /// <param name="resolveType">The resolveing strategy</param>
        /// <returns></returns>
        public Type ResolveDotNetType(string metaclassUri, ResolveType resolveType)
        {
            if (resolveType != ResolveType.OnlyMetaClasses)
            {
                var result = TypeLookup.ToType(metaclassUri);
                if (result != null)
                {
                    return result;
                }
            }

            // Now look into the explicit extents
            foreach (var metaExtent in MetaExtents.Cast<MofUriExtent>())
            {
                var element = metaExtent.TypeLookup.ToType(metaclassUri);
                if (element != null)
                {
                    return element;
                }
            }

            var resolve = ResolveDotNetTypeByMetaWorkspaces(metaclassUri, _Workspace);
            return resolve;
        }

        /// <summary>
        /// Resolves the the given uri by looking through each meta workspace of the workspace
        /// </summary>
        /// <param name="uri">Uri being retrieved</param>
        /// <param name="workspace">Workspace whose meta workspaces were queried</param>
        /// <param name="alreadyVisited">Set of all workspaces already being visited. This avoid unnecessary recursion and unlimited recursion</param>
        /// <returns>Found element or null, if not found</returns>
        private Type ResolveDotNetTypeByMetaWorkspaces(
            string metaclassUri,
            Workspace workspace,
            HashSet<Workspace> alreadyVisited = null)
        {
            alreadyVisited = alreadyVisited ?? new HashSet<Runtime.Workspaces.Workspace>();
            if (alreadyVisited.Contains(workspace))
            {
                return null;
            }

            alreadyVisited.Add(workspace);

            // If still not found, look into the meta workspaces. Nevertheless, no recursion
            var metaWorkspaces = workspace?.MetaWorkspaces;
            if (metaWorkspaces != null)
            {
                foreach (var metaWorkspace in metaWorkspaces)
                {
                    foreach (var metaExtent in metaWorkspace.extent.OfType<MofUriExtent>())
                    {
                        var element = metaExtent.TypeLookup.ToType(metaclassUri);
                        if (element != null)
                        {
                            return element;
                        }
                    }

                    var elementByMeta = ResolveDotNetTypeByMetaWorkspaces(metaclassUri, metaWorkspace, alreadyVisited);
                    if (elementByMeta != null)
                    {
                        return elementByMeta;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the uri of the metaclass by looking through current extent, meta extent and meta workspaces
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <returns>Retrieved meta class uri</returns>
        public string GetMetaClassUri(Type type)
        {
            var result = TypeLookup.ToElement(type);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }

            // Now look into the explicit extents
            foreach (var metaExtent in MetaExtents.OfType<MofExtent>())
            {
                var element = metaExtent.TypeLookup.ToElement(type);
                if (!string.IsNullOrEmpty(element))
                {
                    return element;
                }
            }

            // If still not found, look into the meta workspaces. Nevertheless, no recursion
            var metaWorkspaces = _Workspace?.MetaWorkspaces;
            if (metaWorkspaces != null)
            {
                foreach (var metaWorkspace in metaWorkspaces)
                {
                    foreach (var metaExtent in metaWorkspace.extent.OfType<MofExtent>())
                    {
                        var element = metaExtent.TypeLookup.ToElement(type);
                        if (!string.IsNullOrEmpty(element))
                        {
                            return element;
                        }
                    }
                }
            }

            return null;

        }

        /// <summary>
        /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject. 
        /// An arbitrary object shall be stored into the database
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public object ConvertForSetting(object value)
        {
            return ConvertForSetting(value, this, null);
        }

        /// <summary>
        /// Converts the given value to an element that can be used be for the provider object
        /// </summary>
        /// <param name="value">Value to be set</param>
        /// <param name="extent">Extent being used to create the factory or being used for .Net TypeLookup</param>
        /// <param name="container">Container which will host the newly created object</param>
        /// <returns>The converted object being ready for Provider</returns>
        public static object ConvertForSetting(object value, MofExtent extent, MofObject container)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            if (value == null)
            {
                return null;
            }

            if (DotNetHelper.IsOfPrimitiveType(value) || DotNetHelper.IsOfEnum(value))
            {
                return value;
            }

            if (DotNetHelper.IsOfMofShadow(value))
            {
                var asMofObjectShadow = (MofObjectShadow) value;

                // It is a reference
                var reference = new UriReference
                {
                    Uri = asMofObjectShadow.Uri
                };

                return reference;
            }

            if (DotNetHelper.IsOfMofObject(value))
            {
                var asMofObject = (MofObject) value;
                
                if (asMofObject.Extent == null)
                {
                    if (asMofObject.ProviderObject.Provider == extent?.Provider)
                    {
                        // if the given value is created by the provider, but has not been allocated
                        // to an object until now, it can be used directly. 
                        return asMofObject.ProviderObject;
                    }

                    var result = (MofElement) ObjectCopier.Copy(new MofFactory(extent), asMofObject);
                    /*if (container is IElement containerAsElement)
                    {
                        // Setting a container shall not be done by the copying itself.
                        // Setting the container will be done during the SetProperty
                        // result.Container = containerAsElement;
                    }*/

                    return result.ProviderObject;
                }
                else
                {
                    // It is a reference
                    var reference = new UriReference
                    {
                        Uri = ((MofUriExtent) asMofObject.Extent).uri(asMofObject as IElement)
                    };

                    return reference;
                }
            }

            if (DotNetHelper.IsOfEnumeration(value))
            {
                return ((IEnumerable) value)
                    .Cast<object>()
                    .Select(innerValue => ConvertForSetting(innerValue, extent, container)).ToList();
            }

            // Then, we have a simple dotnet type, that we try to convert. Let's hope, that it works
            if (!(extent is IUriExtent asUriExtent))
            {
                throw new InvalidOperationException(
                    "This element was not created by a factory. So a setting by .Net Object is not possible");
            }

            return ConvertForSetting(DotNetConverter.ConvertToMofObject(asUriExtent, value), extent, container);
        }

        /// <summary>
        /// Converts the object to be set by the data provider. This is the inverse object to ConvertToMofObject. 
        /// An arbitrary object shall be stored into the database
        /// </summary>
        /// <param name="mofObject">The Mofobject for which the element will be created</param>
        /// <param name="childValue">Value to be converted</param>
        /// <returns>The converted object or an exception if the object cannot be converted</returns>
        public static object ConvertForSetting(MofObject mofObject, object childValue)
        {
            var result = ConvertForSetting(childValue, mofObject.ReferencedExtent, mofObject);

            if (result is IProviderObject)
            {
                if (childValue is MofObject childValueAsObject)
                {
                    // Sets the extent of the newly added object which will be associated to the mofObject
                    // This value must be set, so the new information is propagated to the MofObjects
                    childValueAsObject.ReferencedExtent = mofObject.Extent ?? mofObject.ReferencedExtent;
                    childValueAsObject.Extent = mofObject.Extent;
                }
            }

            return result;
        }
    }
}