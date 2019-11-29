using System.Collections.Generic;

namespace DatenMeister.Modules.Forms.Model
{
    public class DropDownFieldData : FieldData
    {
        /// <summary>
        /// Gets the values being used
        /// </summary>
        public IList<ValuePair> values { get; set; } = new List<ValuePair>();

        public const string FieldType = "dropdown";

        public DropDownFieldData() : base(FieldType)
        {
        }

        public DropDownFieldData(string name, string title) : base(FieldType, name, title)
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

        public class ValuePair
        {
            public object value { get; set; }

            public string name { get; set; }

            public ValuePair(object value, string name)
            {
                this.value = value;
                this.name = name;
            }
        }
    }
}