using System;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation.DefaultValue;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;

namespace DatenMeister.Core.Extensions
{
    /// <summary>
    /// This class takes an extent and checks whether the set value of a property equals the
    /// default value.
    /// In case the set value equals the default value, the set value will be removed to
    /// shrinken the size of the data while keeping the values via the default value. 
    /// </summary>
    public class DefaultValueStripper
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        static ILogger logger = new ClassLogger(typeof(DefaultValueStripper));
        
        public void StripDefaultValues(IUriExtent extent, bool dryRun = false)
        {
            // We are going through each item 
            foreach (var element in 
                     extent.elements().GetAllDescendantsIncludingThemselves()
                         .OfType<IObjectAllProperties>()
                         .OfType<IElement>())
            {
                // Checks the set values
                foreach (var property in (element as IObjectAllProperties)!.getPropertiesBeingSet())
                {
                    // Gets default value 
                    var defaultValue = DefaultValueHandler.ReadDefaultValueOfProperty<object?>(element, property);
                    var setValue = element.getOrDefault<object?>(property);

                    if (defaultValue == null || setValue == null)
                    {
                        // One of them is null, so no action is needed
                        continue;
                    }

                    var remove = defaultValue.Equals(setValue);
                    
                    // Check, if there is only a single item without any properties
                    if (setValue is IReflectiveCollection asCollection && asCollection.Count() == 1)
                    {
                        var singleElement = asCollection.ElementAt(0);
                        if (singleElement is IObjectAllProperties properties 
                            && !properties.getPropertiesBeingSet().Any())
                        {
                            remove = true;
                        }
                    }
                    
                    if (remove)
                    {
                        if (dryRun)
                        {
                            Console.WriteLine($"Remove {property} of {element}");
                        }
                        else
                        {
                            Console.WriteLine($"Remove {property} of {element}");
                            element.unset(property);
                        }
                    }
                }
            }
        }
    }
}