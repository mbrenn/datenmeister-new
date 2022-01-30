using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.Extensions.Functions.Transformation
{
    /// <summary>
    /// Converts a reflective sequence of items which contain a cross reference table into a list of items
    /// where the reference are stored as a list within each item. This eases the handling of properties
    /// </summary>
    public class ReferenceReformatter
    {
        private readonly ReferenceFormatterConfiguration _configuration;

        public ReferenceReformatter(ReferenceFormatterConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Converts the elements of the source collection into the target collection according to the given algorithm
        /// </summary>
        /// <param name="source">Source of the elements</param>
        /// <param name="target">Target to which the elements will be added</param>
        public void Convert(IReflectiveCollection source, IReflectiveCollection target)
        {
            var referenceProperty = _configuration.ReferenceProperty
                                    ?? throw new InvalidOperationException("referenceProperty == null");
            var contentProperty = _configuration.ContentProperty
                                  ?? throw new InvalidOperationException("contenttProperty == null");

            var factory = new MofFactory(target);
            foreach (var element in source.OfType<IObject>())
            {
                var created = factory.create(null);
                target.add(created);

                var list = new List<object>();

                // Parses through each properties and adds the values directly or as subitems
                foreach (var property in ((IObjectAllProperties)element).getPropertiesBeingSet())
                {
                    if (_configuration.FixedProperty.Contains(property))
                    {
                        if (element.isSet(property))
                            created.set(property, element.get(property));
                    }
                    else
                    {
                        if (element.isSet(property))
                        {
                            var value = element.get(property);
                            if (string.IsNullOrEmpty(value as string))
                                continue;

                            var inner = factory.create(null);
                            inner.set(referenceProperty, property);
                            inner.set(contentProperty, value);
                            list.Add(inner);
                        }
                    }
                }

                if (list.Count > 0)
                    created.set(_configuration.SubItemProperty ?? string.Empty, list);
            }
        }
    }
}