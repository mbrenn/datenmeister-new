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

    /// <summary>
    /// To be implemented by all fields in which a property value can be injected
    /// </summary>
    public interface IInjectPropertyValue
    {
        /// <summary>
        /// Called if an value shall be injected
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        public void InjectValue(string propertyName, object value);
    }
}