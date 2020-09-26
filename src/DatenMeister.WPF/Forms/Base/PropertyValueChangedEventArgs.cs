using System;

namespace DatenMeister.WPF.Forms.Base
{
    public class PropertyValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the property that has changed
        /// </summary>
        public string PropertyName;

        /// <summary>
        /// Gets or sets the new value
        /// </summary>
        public object? NewValue { get; set; }

        public PropertyValueChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}