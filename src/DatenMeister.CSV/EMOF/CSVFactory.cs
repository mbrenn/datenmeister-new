﻿using System;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.CSV.EMOF
{
    public class CSVFactory : MofFactory
    {
        private readonly IElement _metaClass;

        /// <summary>
        /// Initializes a new instance of the CSVFactory class
        /// </summary>
        /// <param name="extent">Extent, for which the factory is</param>
        public CSVFactory(CSVExtent extent)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the CSVFactory class.
        /// </summary>
        /// <param name="metaClass">Metaclass being used for new objects</param>
        public CSVFactory(IElement metaClass)
        {
            _metaClass = metaClass;
        }

        public override IElement create(IElement metaClass)
        {
            if (metaClass != null && !Equals(metaClass, _metaClass))
            {
                throw new InvalidOperationException("Given metaclass is not of type belonging to extent");
            }

            metaClass = _metaClass;

            return base.create(metaClass);
        }
    }
}