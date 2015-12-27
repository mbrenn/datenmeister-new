using System;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public interface IFactoryMapper
    {
        IFactory FindFactoryFor(Type extentType);
    }
}