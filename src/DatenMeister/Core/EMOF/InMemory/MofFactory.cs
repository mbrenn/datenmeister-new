﻿using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.InMemory
{
    /// <summary>
    ///     Implements the interface according to MOF Core Specificaton 2.5, clause 9.4
    /// </summary>
    public class MofFactory : IFactory
    {
        public IElement package
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IElement create(IElement metaClass)
        {
            return new MofElement(null, metaClass);
        }

        public virtual string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }

        public virtual IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }
    }
}