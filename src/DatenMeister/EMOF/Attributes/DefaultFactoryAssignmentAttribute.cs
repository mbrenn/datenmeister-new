using System;

namespace DatenMeister.EMOF.Attributes
{
    /// <summary>
    /// Performs an assignment of a factory to an extent type
    /// </summary>
    public class DefaultFactoryAssignmentAttribute : Attribute
    {
        public Type FactoryType { get; set; }

        public DefaultFactoryAssignmentAttribute(Type factoryType)
        {
            FactoryType = factoryType;
        }
    }
}