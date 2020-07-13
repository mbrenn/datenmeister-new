using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Runtime.DynamicFunctions;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    public class Workspace : IWorkspace, IObject, IUriResolver, IObjectAllProperties
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(Workspace));

        private readonly object _syncObject = new object();

        private readonly List<IExtent> _extent = new List<IExtent>();

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly List<ITag> _properties = new List<ITag>();

        /// <summary>
        /// Stores a list of meta workspaces that are associated to the given workspace
        /// The metaworkspaces are requested to figure out meta classes
        /// </summary>
        public List<Workspace> MetaWorkspaces { get; } = new List<Workspace>();

        /// <summary>
        /// Adds plugins which allow additional extents to an extent
        /// </summary>
        public List<IEnumerable<IExtent>> ExtentPlugins = new List<IEnumerable<IExtent>>();
        
        /// <summary>
        /// Stores the dynamic function managers
        /// </summary>
        public DynamicFunctionManager DynamicFunctionManager { get; } = new DynamicFunctionManager();

        /// <summary>
        /// Adds a meta workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added as a meta workspace</param>
        public void AddMetaWorkspace(Workspace workspace)
        {
            lock (_syncObject)
            {
                MetaWorkspaces.Add(workspace);
            }
        }

        /// <summary>
        /// Gets a list the cache which stores the filled types
        /// </summary>
        internal List<object> FilledTypeCache { get; } = new List<object>();

        public string id { get; }

        public string annotation { get; set; }

        /// <summary>
        /// Gets the extents. The source of the extent list is the _extent combined with the
        /// enumeration of plugins.
        /// </summary>
        public IEnumerable<IExtent> extent
        {
            get
            {
                foreach (var localExtent in _extent)
                {
                    yield return localExtent;
                }

                foreach (var pluginExtent in ExtentPlugins.SelectMany(plugin => plugin))
                {
                    yield return pluginExtent;
                }
            }
        }

        public IEnumerable<ITag> properties => _properties;

        public Workspace(string id, string annotation = "")
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.annotation = annotation;
        }

        public TFilledType Create<TFiller, TFilledType>()
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            lock (_syncObject)
            {
                var filledType = Get<TFilledType>();
                if (filledType != null)
                {
                    return filledType;
                }

                // Not found, we need to fill it on our own... Congratulation
                var filler = new TFiller();
                filledType = new TFilledType();

                // Go through all extents of this datalayer
                foreach (var oneExtent in extent)
                {
                    filler.Fill(oneExtent.elements(), filledType);
                }

                // Adds it to the database
                FilledTypeCache.Add(filledType);
                return filledType;
            }
        }

        public void ClearCache()
        {
            lock (_syncObject)
            {
                FilledTypeCache.Clear();
            }
        }

        /// <summary>
        /// Adds an extent to the workspace
        /// </summary>
        /// <param name="newExtent">The extent to be added</param>
        public void AddExtent(IUriExtent newExtent)
        {
            var asMofExtent = (MofExtent) newExtent;
            if (newExtent == null) throw new ArgumentNullException(nameof(newExtent));
            if (asMofExtent.Workspace != null)
            {
                Logger.Fatal($"The extent is already assigned to a workspace: {newExtent.contextURI()}");
                throw new InvalidOperationException("The extent is already assigned to a workspace");
            }

            lock (_syncObject)
            {
                if (extent.Any(x => (x as IUriExtent)?.contextURI() == newExtent.contextURI()))
                {
                    Logger.Fatal($"Extent with uri {newExtent.contextURI()} is already added to the given workspace");
                    throw new InvalidOperationException($"Extent with uri {newExtent.contextURI()} is already added to the given workspace");
                }

                Logger.Debug($"Added extent to workspace: {newExtent.contextURI()} --> {id}");
                _extent.Add(newExtent);
                asMofExtent.Workspace = this;
            }
        }

        /// <summary>
        /// Removes the extent with the given uri out of the database
        /// </summary>
        /// <param name="uri">Uri of the extent</param>
        /// <returns>true, if the object can be deleted</returns>
        public bool RemoveExtent(string uri)
        {
            lock (_syncObject)
            {
                var found = _extent.FirstOrDefault(
                    x => x is IUriExtent uriExtent
                         && uriExtent.contextURI() == uri);

                if (found != null)
                {
                    _extent.Remove(found);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes the extent from the workspace
        /// </summary>
        /// <param name="extentForRemoval">Extent to be removed</param>
        /// <returns>true, if the extent could be removed</returns>
        public bool RemoveExtent(IExtent extentForRemoval)
        {
            lock (_syncObject)
            {
                return _extent.Remove(extentForRemoval);
            }
        }

        public TFilledType? Get<TFilledType>()
            where TFilledType : class, new()
        {
            lock (_syncObject)
            {
                // Looks into the cache for the filledtypes
                foreach (var value in FilledTypeCache)
                {
                    if (value is TFilledType filledType)
                    {
                        return filledType;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the element of the current workspace and performs in addition a conversion function
        /// </summary>
        /// <typeparam name="TFilledType">Class to be looked after</typeparam>
        /// <param name="function"></param>
        /// <returns>Found function</returns>
        public IElement? Get<TFilledType>(Func<TFilledType, IElement> function)
            where TFilledType : class, new()
        {
            var result = Get<TFilledType>();
            if (result == null)
            {
                return null;
            }

            return function(result);
        }

        /// <summary>
        /// Creates the element of the current workspace and performs in addition a conversion function.
        /// The elements are cached for performancewise improved handling
        /// </summary>
        /// <typeparam name="TFilledType">Class to be looked after</typeparam>
        /// <param name="function"></param>
        /// <returns>Found function</returns>
        public IElement? Create<TFilledType>(Func<TFilledType, IElement> function)
            where TFilledType : class, new()
        {
            var result = Get<TFilledType>();
            if (result == null)
            {
                return null;
            }

            return function(result);
        }

        /// <summary>
        /// Gets a property by querying all meta workspaces
        /// </summary>
        /// <typeparam name="TFilledType">Property to be queried</typeparam>
        /// <returns>The property being queried</returns>
        public TFilledType? GetFromMetaWorkspace<TFilledType>(
            MetaRecursive metaRecursive = MetaRecursive.JustOne)
            where TFilledType : class, new()
        {
            lock (_syncObject)
            {
                var open = new List<Workspace>(MetaWorkspaces);
                var visited = new List<Workspace>();
                while (open.Count > 0)
                {
                    var meta = open[0];
                    open.RemoveAt(0);
                    visited.Add(meta);

                    var result = meta.Get<TFilledType>();
                    if (result != null)
                    {
                        return result;
                    }

                    // Adds the meta workspaces of the meta workspace to the list to be analyzed
                    if (metaRecursive == MetaRecursive.Recursively)
                    {
                        var newMetaWorkspaces = meta.MetaWorkspaces;
                        foreach (var newMeta in newMetaWorkspaces)
                        {
                            if (!visited.Contains(newMeta))
                            {
                                open.Add(newMeta);
                            }
                        }
                    }
                }
            }

            return default;
        }

        public void Set<TFilledType>(TFilledType value) where TFilledType : class, new()
        {
            lock (_syncObject)
            {
                FilledTypeCache.Add(value);
            }
        }

        public override string ToString() =>
            !string.IsNullOrEmpty(annotation)
                ? $"({id}) {annotation}"
                : $"({id})";

        public IEnumerable<string> getPropertiesBeingSet()
        {
            yield return "id";
        }

        public bool @equals(object? other) => throw new NotImplementedException();

        public object? get(string property)
        {
            if (property == "id")
            {
                return id;
            }

            throw new InvalidOperationException($"Given property {id} is not set.");
        }

        public void set(string property, object? value)
        {
            throw new NotImplementedException();
        }

        public bool isSet(string property) => property == "id";

        public void unset(string property)
        {
            throw new NotImplementedException();
        }

        public object? Resolve(string uri, ResolveType resolveType, bool traceFailing)
        {
            var result = _extent
                .Select(theExtent =>
                    (theExtent as IUriResolver)?.Resolve(uri, resolveType | ResolveType.NoWorkspace, false))
                .FirstOrDefault(found => found != null);
            if (result == null && traceFailing)
            {
                Logger.Trace($"URI not resolved: {uri}");
            }

            return result;
        }

        public IElement? ResolveById(string elementId)
        {
            return _extent.Select(theExtent => (theExtent as IUriResolver)?.ResolveById(elementId)).FirstOrDefault(found => found != null);
        }
    }
}