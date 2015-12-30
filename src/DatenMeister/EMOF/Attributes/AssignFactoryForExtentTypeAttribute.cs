using System;

namespace DatenMeister.EMOF.Attributes
{
    /// <summary>
    /// Performs an assignment of a factory to an extent type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssignFactoryForExtentTypeAttribute : Attribute
    {
        public Type FactoryType { get; private set; }

        public AssignFactoryForExtentTypeAttribute(Type factoryType)
        {
            FactoryType = factoryType;
        }
    }
}