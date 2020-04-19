using System;
using System.Collections.Generic;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.Validators;
using DatenMeister.Runtime;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Defines the class for a view in list
    /// </summary>
    public class FormDefinition
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public FormDefinitionMode Mode { get; set; }

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets the corresponding element
        /// </summary>
        public IObject? Element { get; }

        /// <summary>
        /// Stores the list of validators
        /// </summary>
        public readonly List<IElementValidator> Validators = new List<IElementValidator>();

        /// <summary>
        /// Gets the view definitions that are application for the complete extent form
        /// </summary>
        public List<ViewExtension> ViewExtensions { get; set; } = new List<ViewExtension>();

        /// <summary>
        /// Gets or sets the function that will receive a list of view extensions dependent on the form for the tab being used
        /// This function is called by the ItemExplorerControl to figure the valid extensions
        /// </summary>
        public Func<IElement, IEnumerable<ViewExtension>>? TabViewExtensionsFunction { get; set; }

        /// <summary>
        /// Initializes a new instance of the new ViewDefinition class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="element"></param>
        /// <param name="mode">Stores the type as given</param>
        public FormDefinition(string name, IObject? element, FormDefinitionMode mode = FormDefinitionMode.Specific)
        {
            if (element == null && mode == FormDefinitionMode.Specific)
            {
                Mode = FormDefinitionMode.Default;
                Name = name;
            }
            else
            {
                Name = name;
                Element = element;
                Mode = mode;
            }
        }

        public FormDefinition(FormDefinitionMode mode) : this(string.Empty, null, mode)
        {
        }


        public FormDefinition(IObject? element, FormDefinitionMode mode = FormDefinitionMode.Specific)
        {
            if (element == null && mode == FormDefinitionMode.Specific)
            {
                Mode = FormDefinitionMode.Default;
            }
            else if (element != null)
            {
                Name = element.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) ?? string.Empty;
                Element = element;
                Mode = mode;
            }
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
