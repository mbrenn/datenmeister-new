using System;
using System.Collections.Generic;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    /// <typeparam name="T">Type of the extents being handled</typeparam>
    public class Workspace
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

        public override string ToString()
        {
            return !string.IsNullOrEmpty(annotation) 
                ? $"({id}) {annotation}" 
                : $"({id})";
        }
    }
}