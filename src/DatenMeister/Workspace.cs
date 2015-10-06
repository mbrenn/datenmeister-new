using DatenMeister.EMOF.Interface.Extension;
using DatenMeister.EMOF.Interface.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    public class Workspace<T> where T : IExtent
    {
        private string _id;

        private string _annotation;

        private List<ITag> _properties = new List<ITag>();

        private List<T> _extent = new List<T>();
        
        public IEnumerable<T> extent
        {
            get { return _extent; }
        }

        public IEnumerable<ITag> properties
        {
            get { return _properties; }
        }

        public Workspace(string id, string annotation = null)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _id = id;
            _annotation = annotation;
        }

        public void AddExtent(T extent)
        {
            _extent.Add(extent);
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_annotation))
            {
                return $"({_id}) {_annotation}";
            }
            else
            {
                return $"({_id})";
            }
        }
    }

}
