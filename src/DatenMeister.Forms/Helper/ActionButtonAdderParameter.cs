﻿using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using System;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.Helper
{
    /// <summary>
    /// Defines the parameter for the ActionButtonToFormadder
    /// </summary>
    public record ActionButtonAdderParameter(string ActionName, string Title) : FormCreationContext
    {
        /// <summary>
        /// The action name being used. 
        /// </summary>
        public string ActionName { get; set; } = ActionName;

        /// <summary>
        /// The title being used. 
        /// </summary>
        public string Title { get; set; } = Title;

        /// <summary>
        /// Gets or sets a predicate that can be used as an additional filtering option.
        /// If the method is not set, the element will be considered as fitting.
        /// If the element is set, then the predicate must return true, to add the filter element 
        /// </summary>
        public Func<IObject?, bool>? PredicateForElement { get; set; }

        /// <summary>
        /// Gets or sets the delegate that will be called, when the 
        /// the filter is evaluated. This allows setting a breakpoint for the debugger
        /// during issue finding
        /// </summary>
        public Action? OnCallSuccess { get; set; }

        /// <summary>
        /// Gets or sets the position at which the button shall be added
        /// </summary>
        public int ActionButtonPosition { get; set; } = -1;
    }
}