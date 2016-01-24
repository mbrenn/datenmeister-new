using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Extension;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister
{
    public class Workspace<T> where T : IExtent
    {
        private readonly List<T> _extent = new List<T>();

        private readonly List<ITag> _properties = new List<ITag>();

        public Workspace(string id, string annotation = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.id = id;
            this.annotation = annotation;
        }

        public string id { get; }

        public string annotation { get; }

        public IEnumerable<T> extent
        {
            get { return _extent; }
        }

        public IEnumerable<ITag> properties
        {
            get { return _properties; }
        }

        public void AddExtent(T extent)
        {
            _extent.Add(extent);
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(annotation) 
                ? $"({id}) {annotation}" 
                : $"({id})";
        }
    }
}