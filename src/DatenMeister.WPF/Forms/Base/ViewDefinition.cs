﻿using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base.ViewExtensions;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Defines the class for a view in list
    /// </summary>
    public class ViewDefinition
    { 
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public ViewDefinitionMode Mode { get; set; }

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the corresponding element
        /// </summary>
        public IElement Element { get; }

        /// <summary>
        /// Initializes a new instance of the new ViewDefinition class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="element"></param>
        /// <param name="mode">Stores the type as given</param>
        public ViewDefinition(string name, IElement element, ViewDefinitionMode mode = ViewDefinitionMode.Specific)
        {
            if (element == null)
            {
                Mode = ViewDefinitionMode.Default;
                Name = name;
            }
            else
            {
                Name = name;
                Element = element;
                Mode = mode;
            }
        }

        public ViewDefinition(IElement element, ViewDefinitionMode mode = ViewDefinitionMode.Specific)
        {
            if (element == null)
            {
                Mode = ViewDefinitionMode.Default;
            }
            else
            {
                Name = element.getOrDefault<string>(_UML._CommonStructure._NamedElement.name);
                Element = element;
                Mode = mode;
            }
        }

        public List<ViewExtension> ViewExtensions { get; } = new List<ViewExtension>();

        public ViewDefinition(ViewDefinitionMode mode) : this (null, null, mode)
        {   
        }

        /// <summary>
        /// Converts the view definition to a string
        /// </summary>
        /// <returns>The converted string</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}