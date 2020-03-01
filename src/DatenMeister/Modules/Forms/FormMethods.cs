using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Forms
{
    /// <summary>
    /// Contains some helper methods for forms
    /// </summary>
    public class FormMethods
    {
        /// <summary>
        /// Performs a verification of the form and returns false, if the form is not in a valid state
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <returns>true, if the form is valid</returns>
        public bool ValidateForm(IObject form)
        {
            var fields = form.getOrDefault<IReflectiveCollection>(_FormAndFields._DetailForm.field);
            if (fields != null)
            {
                if (!ValidateFields(fields)) return false;
            }

            var tabs = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            if (tabs != null)
            {
                foreach (var tab in tabs.OfType<IObject>())
                {
                    if (!ValidateForm(tab)) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks, if there is a duplicated name of the fields
        /// </summary>
        /// <param name="fields">Fields to be enumerated</param>
        /// <returns>true, if there are no duplications</returns>
        private static bool ValidateFields(IEnumerable fields)
        {
            var set = new HashSet<string>();
            foreach (var field in fields.OfType<IObject>())
            {
                var name = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                if (set.Contains(name))
                {
                    return false;
                }

                set.Add(name);
            }

            return true;
        }
    }
}