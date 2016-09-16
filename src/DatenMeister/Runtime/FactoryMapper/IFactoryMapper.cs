using System;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public interface IFactoryMapper
    {
        /// <summary>
        /// Adds a mapping of a factory to a certain extent type
        /// </summary>
        /// <param name="type">Type of the extent</param>
        /// <param name="createFunc">Function, which creates the factory</param>
        void AddMapping(Type type, Func<ILifetimeScope, IFactory> createFunc);
        IFactory FindFactoryFor(ILifetimeScope scope, Type extentType);
    }
}