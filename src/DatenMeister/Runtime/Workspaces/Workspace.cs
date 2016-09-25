using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Filler;

namespace DatenMeister.Runtime.Workspaces
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    /// <typeparam name="T">Type of the extents being handled</typeparam>
    public class Workspace : IWorkspace
    {
        private readonly object _syncObject = new object();

        private readonly List<IExtent> _extent = new List<IExtent>();

        private readonly List<ITag> _properties = new List<ITag>();

        /// <summary>
        /// Gets a list the cache which stores the filled types
        /// </summary>
        internal List<object> FilledTypeCache { get; } = new List<object>();

        /// <summary>
        /// Gets or sets the meta workspace for the given
        /// </summary>
        public Workspace MetaWorkspace { get; set; }

        public string id { get; }

        public string annotation { get; }

        public IList<IExtent> extent => _extent;

        public IEnumerable<ITag> properties => _properties;

        public object SyncObject => _syncObject;

        public Workspace(string id, string annotation = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.id = id;
            this.annotation = annotation;
        }

        public void AddExtent(IUriExtent newExtent)
        {
            lock (_syncObject)
            {
                _extent.Add(newExtent);
            }
        }

        public TFilledType Create<TFiller, TFilledType>()
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            lock (FilledTypeCache)
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
            lock (FilledTypeCache)
            {
                FilledTypeCache.Clear();
            }
        }

        public TFilledType Get<TFilledType>()
            where TFilledType : class, new()
        {
            lock (FilledTypeCache)
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

        public void Set<TFilledType>(TFilledType value) where TFilledType : class, new()
        {
            lock (FilledTypeCache)
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
    }
}