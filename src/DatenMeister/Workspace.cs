using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Extension;
using DatenMeister.EMOF.Interface.Identifiers;

namespace DatenMeister
{
    public class Workspace<T> where T : IExtent
    {
        private object syncObject = new object();
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

        public IEnumerable<T> extent => _extent;

        public IEnumerable<ITag> properties => _properties;

        public void AddExtent(T extent)
        {
            lock (syncObject)
            {
                _extent.Add(extent);
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(annotation) 
                ? $"({id}) {annotation}" 
                : $"({id})";
        }

        /// <summary>
        /// Removes the extent with the given uri out of the database
        /// </summary>
        /// <param name="uri">Uri of the extent</param>
        /// <returns>true, if the object can be deleted</returns>
        public bool RemoveExtent(string uri)
        {
            lock (syncObject)
            {
                var found = _extent.FirstOrDefault(x =>
                {
                    var uriExtent = x as IUriExtent;
                    return uriExtent != null && uriExtent.contextURI() == uri;
                });

                if (found != null)
                {
                    _extent.Remove(found);
                    return true;
                }
            }

            return false;
        }
    }
}