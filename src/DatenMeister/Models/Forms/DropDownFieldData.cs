using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    public class DropDownFieldData : FieldData
    {
        /// <summary>
        /// Gets the values being used
        /// </summary>
        public IList<ValuePair> values { get; set; } = new List<ValuePair>();
        
        /// <summary>
        /// If set, the values will be set from the enumeration type. 
        /// </summary>
        public IElement? valuesByEnumeration { get; set; }

        public DropDownFieldData() : base()
        {
        }

        public DropDownFieldData(string name, string title) : base(name, title)
        {
            
        }

        /// <summary>
        /// Adds a specific value for the dropdown form
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <param name="nameOfValue">Name of the item being shown to the user</param>
        public void AddValue(object value, string nameOfValue)
        {
            var pair = new ValuePair(value, nameOfValue);

            values.Add(pair);
        }
    }

    /// <summary>
    /// Defines the value 
    /// </summary>
    public class ValuePair
    {
        /// <summary>
        /// The value which is used when the user clicks the item
        /// </summary>
        public object value { get; set; }

        /// <summary>
        /// The value which is used to show to the user
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Initializes a new instance of the ValuePair class
        /// </summary>
        /// <param name="value">Value to be set</param>
        /// <param name="name">Name to be set</param>
        public ValuePair(object value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }
}