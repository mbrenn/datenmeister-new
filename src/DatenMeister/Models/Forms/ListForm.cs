using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Forms
{
    public class ListForm : Form
    {
        public ListForm()
        {
        }

        public ListForm(string name) : base(name)
        {
        }

        public ListForm(string name, params FieldData[] fieldsToBeAdded) : base(name, fieldsToBeAdded)
        {
        }

        /// <summary>
        ///     Stores an enumeration of default types that can be used for creation
        /// </summary>
        public IList<IElement> defaultTypesForNewElements { get; set; }
    }
}