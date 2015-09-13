﻿using DatenMeister.MOF.Interface.Common;
using DatenMeister.MOF.Interface.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Interface.Extension
{
    /// <summary>
    /// Defines the Tag interface as defined in MOF CoreSpecification 2.5
    /// </summary>
    // TODO: Inherit from IElement
    public interface ITag
    {
        string name
        {
            get;
            set;
        }

        string value
        {
            get;
            set;
        }

        IReflectiveCollection elements
        {
            get;
            set;
        }

        IElement owner
        {
            get;
            set;
        }
    }
}
