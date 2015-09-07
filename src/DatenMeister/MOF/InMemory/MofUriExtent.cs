using DatenMeister.MOF.Interface.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.MOF.Interface.Common;
using DatenMeister.MOF.Interface.Reflection;

namespace DatenMeister.MOF.InMemory
{
    public class MofUriExtent : IUriExtent
    {
        /// <summary>
        /// Stores all the elements
        /// </summary>
        List<object> _elements = new List<object>();
        
        private string _contextUri;

        public MofUriExtent(string uri)
        {
            _contextUri = uri;
        }

        public string contextURI()
        {
            return this._contextUri;
        }

        public IElement element(string uri)
        {
            var uriAsUri = new Uri(uri);
            
            throw new NotImplementedException();
        }

        public string uri(IElement element)
        {
            if ( element == null )
            {
                throw new ArgumentNullException("element");
            }

            var elementAsObject = element as MofObject;
            if ( elementAsObject == null)
            {
                throw new InvalidOperationException("element is not of type MofObject. Element is: " + element.ToString());
            }
            
            return _contextUri + "#" + elementAsObject.guid.ToString();
        }

        public bool useContainment()
        {
            return false;
        }

        IReflectiveSequence IExtent.elements()
        {
            return new MofReflectiveSequence(_elements);
        }
    }
}
