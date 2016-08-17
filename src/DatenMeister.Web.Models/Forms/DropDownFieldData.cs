using System.Collections.Generic;

namespace DatenMeister.Web.Models.Forms
{
    public class DropDownFieldData : FieldData
    {
        /// <summary>
        /// Gets the values being used 
        /// </summary>
        public IList<ValuePair> values { get; set; } = new List<ValuePair>();

        public DropDownFieldData() : base("dropdown")
        {
        }

        public DropDownFieldData(string name, string title) : base("dropdown", name, title)
        {
            
        }

        public void AddValue(object value, string name)
        {
            var pair = new ValuePair(value, name);

            values.Add(pair);
        }

        public class ValuePair
        {
            public object value;

            public string name;

            public ValuePair(object value, string name)
            {
                this.value = value;
                this.name = name;
            }
        }
    }
}