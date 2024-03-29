﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.DynamicFunctions;

// ReSharper disable InconsistentNaming

namespace DatenMeister.Core.Runtime.Workspaces
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    public class Workspace : IWorkspace, IObject, IUriResolver, IObjectAllProperties
    {
        private static readonly ClassLogger Logger = new(typeof(Workspace));

        private readonly List<IExtent> _extent = new();

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly List<ITag> _properties = new();

        private readonly object _syncObject = new();

        /// <summary>
        ///     Adds plugins which allow additional extents to an extent
        /// </summary>
        public List<IEnumerable<IExtent>> ExtentFactory = new();

        public Workspace(string id, string annotation = "")
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.annotation = annotation;
        }

        /// <summary>
        /// Gets or sets the information whether the workspace is a dynamic workspace
        /// If it is so, then it is skipped during resolvings
        /// </summary>
        public bool IsDynamicWorkspace { get; set; }

        /// <summary>
        ///     Gets a list the cache which stores the filled types
        /// </summary>
        internal List<object> FilledTypeCache { get; } = new();

        public string annotation { get; set; }

        public IEnumerable<ITag> properties => _properties;

        public bool equals(object? other)
        {
            throw new NotImplementedException();
        }

        public object get(string property)
        {
            if (property == "id") return id;

            throw new InvalidOperationException($"Given property {id} is not set.");
        }

        public void set(string property, object? value)
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

        public IEnumerable<string> getPropertiesBeingSet()
        {
            yield return "id";
        }

        public object? Resolve(string uri, ResolveType resolveType, bool traceFailing = true, string? workspace = null)
        {
            var result = extent
                .Select(theExtent =>
                    (theExtent as IUriResolver)?.Resolve(uri, resolveType | ResolveType.NoWorkspace, false, workspace))
                .FirstOrDefault(found => found != null);
            if (result == null && traceFailing) Logger.Trace($"URI not resolved: {uri}");

            return result;
        }

        public IElement? ResolveById(string elementId)
        {
            lock (_syncObject)
            {
                return _extent.Select(theExtent => (theExtent as IUriResolver)?.ResolveById(elementId))
                    .FirstOrDefault(found => found != null);
            }
        }

        /// <summary>
        /// Stores a list of meta workspaces that are associated to the given workspace
        /// The metaworkspaces are requested to figure out meta classes
        /// </summary>
        public List<Workspace> MetaWorkspaces { get; } = new();

        /// <summary>
        /// Stores the dynamic function managers
        /// </summary>
        public DynamicFunctionManager DynamicFunctionManager { get; } = new();

        public string id { get; }

        /// <summary>
        /// Gets the extents. The source of the extent list is the _extent combined with the
        /// enumeration of plugins.
        /// </summary>
        public IEnumerable<IExtent> extent
        {
            get
            {
                var result = new List<IExtent>();
                lock (_syncObject)
                {
                    foreach (var localExtent in _extent)
                    {
                        result.Add(localExtent);
                    }
                }

                foreach (var pluginExtent in ExtentFactory.SelectMany(plugin => plugin))
                {
                    result.Add(pluginExtent);
                }

                return result;
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
        ///     Adds a meta workspace
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
                    throw new InvalidOperationException(
                        $"Extent with uri {newExtent.contextURI()} is already added to the given workspace");
                }

                if (newExtent.contextURI() == string.Empty)
                {
                    Logger.Error($"Empty contextUri() for Extent given");
                    Debugger.Break();
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

        public override string ToString() =>
            !string.IsNullOrEmpty(annotation)
                ? $"({id}) {annotation}"
                : $"({id})";
    }
}