using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    /// <typeparam name="T">Type of the extents being handled</typeparam>
    public class Workspace : IWorkspace, IObject, IUriResolver
    {
        private readonly object _syncObject = new object();

        private readonly List<IExtent> _extent = new List<IExtent>();

        private readonly List<ITag> _properties = new List<ITag>();

        /// <summary>
        /// Stores a list of meta workspaces that are associated to the given workspace
        /// The metaworkspaces are requested to figure out meta classes
        /// </summary>
        public List<Workspace> MetaWorkspaces { get; } = new List<Workspace>();

        /// <summary>
        /// Adds a meta workspace
        /// </summary>
        /// <param name="workspace">Workspace to be added as a meta workspace</param>
        public void AddMetaWorkspace(Workspace workspace)
        {
            lock (MetaWorkspaces)
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

        public IEnumerable<IExtent> extent => _extent;

        public IEnumerable<ITag> properties => _properties;

        public object SyncObject => _syncObject;

        public Workspace(string id, string annotation = null)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.annotation = annotation;
        }

        public TFilledType Create<TFiller, TFilledType>()
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            lock (SyncObject)
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
            lock (SyncObject)
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
            var asMofExtent = (MofExtent)newExtent;
            if (newExtent == null) throw new ArgumentNullException(nameof(newExtent));
            if (asMofExtent.Workspace != null)
            {
                throw new InvalidOperationException("The extent is already assigned to a workspace");
            }

            lock (SyncObject)
            {
                if (extent.Any(x => (x as IUriExtent)?.contextURI() == newExtent.contextURI()))
                {
                    throw new InvalidOperationException($"Extent with uri {newExtent.contextURI()} is already added to the given workspace");
                }

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
            lock (SyncObject)
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

        public TFilledType Get<TFilledType>()
            where TFilledType : class, new()
        {
            lock (SyncObject)
            {
                // Looks into the cache for the filledtypes
                foreach (var value in FilledTypeCache)
                {
                    if (value is TFilledType)
                    {
                        return value as TFilledType;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a property by querying all meta workspaces
        /// </summary>
        /// <typeparam name="TFilledType">Property to be queried</typeparam>
        /// <returns>The property being queried</returns>
        public TFilledType GetFromMetaWorkspace<TFilledType>(MetaRecursive metaRecursive = MetaRecursive.JustOne)
            where TFilledType : class, new()
        {
            lock (SyncObject)
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

            return null;
        }

        public void Set<TFilledType>(TFilledType value) where TFilledType : class, new()
        {
            lock (SyncObject)
            {
                FilledTypeCache.Add(value);
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(annotation) 
                ? $"({id}) {annotation}" 
                : $"({id})";
        }

        public bool @equals(object other)
        {
            throw new NotImplementedException();
        }

        public object get(string property)
        {
            if (property == "id")
            {
                return id;
            }

            throw new InvalidOperationException($"Given property {id} is not set.");
        }

        public void set(string property, object value)
        {
            throw new NotImplementedException();
        }

        public bool isSet(string property)
        {
            return property == "id";
        }

        public void unset(string property)
        {
            throw new NotImplementedException();
        }

        public IElement Resolve(string uri, ResolveType resolveType)
        {
            return _extent.Select(theExtent => (theExtent as IUriResolver)?.Resolve(uri, resolveType | ResolveType.NoWorkspace)).FirstOrDefault(found => found != null);
        }

        public IElement ResolveById(string id)
        {
            return _extent.Select(theExtent => (theExtent as IUriResolver)?.ResolveById(id)).FirstOrDefault(found => found != null);
        }
    }
}