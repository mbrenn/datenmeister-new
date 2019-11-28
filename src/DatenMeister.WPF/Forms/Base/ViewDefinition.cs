using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Modules.ViewFinder;
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
        public IObject Element { get; }

        /// <summary>
        /// Initializes a new instance of the new ViewDefinition class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="element"></param>
        /// <param name="mode">Stores the type as given</param>
        public ViewDefinition(string name, IObject element, ViewDefinitionMode mode = ViewDefinitionMode.Specific)
        {
            if (element == null && mode == ViewDefinitionMode.Specific)
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

        public ViewDefinition(IObject element, ViewDefinitionMode mode = ViewDefinitionMode.Specific)
        {
            if (element == null && mode == ViewDefinitionMode.Specific)
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

        /// <summary>
        /// Gets the view definitions that are application for the complete extent form
        /// </summary>
        public List<ViewExtension> ViewExtensions { get; set; } = new List<ViewExtension>();

        /// <summary>
        /// Gets or sets the function that will receive a list of view extensions dependent on the form for the tab being used
        /// This function is called by the ItemExplorerControl to figure the valid extensions
        /// </summary>
        public Func<IElement, IEnumerable<ViewExtension>> TabViewExtensionsFunction { get; set; }

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
