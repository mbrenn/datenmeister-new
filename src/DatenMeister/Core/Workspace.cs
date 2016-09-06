﻿using System;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Extension;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core
{
    /// <summary>
    /// Defines a workspace according to the Mof specification
    /// MOF Facility Object Lifecycle (MOFFOL)
    /// </summary>
    /// <typeparam name="T">Type of the extents being handled</typeparam>
    public class Workspace<T> where T : IExtent
    {
        private readonly object _syncObject = new object();

        private readonly List<T> _extent = new List<T>();

        private readonly List<ITag> _properties = new List<ITag>();

        public string id { get; }

        public string annotation { get; }

        public IList<T> extent => _extent;

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

        public void AddExtent(T newExtent)
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