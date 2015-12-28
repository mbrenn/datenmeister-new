using System;
using System.Linq;
using System.Reflection;
using DatenMeister.EMOF.Attributes;
using DatenMeister.Runtime.FactoryMapper;

namespace DatenMeister.Full.Integration
{
    public static class Integration
    {
        public static void PerformAutomaticMappingByAttribute(this DefaultFactoryMapper mapper)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            foreach (var type in types)
            {
                foreach (
                    var customAttribute in type.GetCustomAttributes(typeof (DefaultFactoryAssignmentAttribute), false))
                {
                    var factoryAssignmentAttribute = customAttribute as DefaultFactoryAssignmentAttribute;
                    mapper.AddMapping(type, factoryAssignmentAttribute.FactoryType);
                }
            }

        }
    }
}