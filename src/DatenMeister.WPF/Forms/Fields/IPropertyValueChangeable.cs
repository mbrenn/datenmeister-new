using System;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Fields
{
    /// <summary>
    /// This interface has to be defined by all detail fields which support the
    /// event that the content of a detail field has been changed
    /// </summary>
    public interface IPropertyValueChangeable
    {
        /// <summary>
        /// This event will be called when a property value is changed
        /// </summary>
        public event EventHandler<PropertyValueChangedEventArgs>? PropertyValueChanged;
    }
}