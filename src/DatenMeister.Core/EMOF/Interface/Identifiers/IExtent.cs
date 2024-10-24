﻿using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Interface.Identifiers
{
    /// <summary>
    ///     Defines the interface for the Extent as defined in MOF Specification 2.5, Chapter 10.2
    /// </summary>
    public interface IExtent : IObject
    {
        bool useContainment();

        IReflectiveSequence elements();
    }
}