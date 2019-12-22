using System;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder.Helper
{
    public class FormHelper
    {
        /// <summary>
        /// Looks for the given field in the form and returns it, if it is available
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement GetField(IElement form, string fieldName)
        {
            if (_FormAndFields._DetailForm.field != _FormAndFields._ListForm.field)
                throw new InvalidOperationException("Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");
            
            var fields = form.get<IReflectiveCollection>(_FormAndFields._DetailForm.field);
            return fields
                .WhenPropertyHasValue(_FormAndFields._FieldData.name, fieldName)
                .OfType<IElement>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the list tab for a certain property name 
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="propertyName">Name of the property to which the propertyname shall belong</param>
        /// <returns>The found element</returns>
        public static IElement GetListTabForPropertyName(IElement form, string propertyName)
        {
            if (_FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab)
                throw new InvalidOperationException("Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");

            var tabs = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();

            foreach (var tab in tabs.OfType<IElement>())
            {
                if ( ClassifierMethods.IsSpecializedClassifierOf(tab.getMetaClass(), formAndFields.__ListForm))
                {
                    var property = tab.getOrDefault<string>(_FormAndFields._ListForm.property);
                    if (property == propertyName)
                    {
                        return tab;
                    }
                }
            }

            return null;
        }
    }
}