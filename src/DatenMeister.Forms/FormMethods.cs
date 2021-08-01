using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms
{
    /// <summary>
    /// Contains some helper methods for forms
    /// </summary>
    public class FormMethods
    {
        /// <summary>
        /// Logger being used
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(FormMethods));

        /// <summary>
        /// Stores the workspacelogic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        public FormMethods(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Performs a verification of the form and returns false, if the form is not in a valid state
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <returns>true, if the form is valid</returns>
        public static bool ValidateForm(IObject form)
        {
            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            if (fields != null)
            {
                if (!ValidateFields(fields)) return false;
            }

            var tabs = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab);
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
            var randomGuid = Guid.NewGuid();
            var set = new HashSet<string>();
            foreach (var field in fields.OfType<IObject>())
            {
                var preName = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
                var isAttached = field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isAttached);
                var name = isAttached ? randomGuid  + preName : preName;
                
                if (set.Contains(name) && !string.IsNullOrEmpty(name))
                {
                    Logger.Warn($"Field '{name}' is included twice. Validation of form failed");
                    return false;
                }

                set.Add(name);
            }

            return true;
        }

        /// <summary>
        /// Checks if the given element already has a metaclass within the form
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>true, if the form already contains a metaclass form</returns>
        public static bool HasMetaClassFieldInForm(IObject form)
        {
            var formAndFields = _DatenMeister.TheOne.Forms;
            return form
                .get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field)
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.equals(formAndFields.__MetaClassElementFieldData) ?? false);
        }

        /// <summary>
        /// Checks if the given element already has a metaclass within the form
        /// </summary>
        /// <param name="extent">Defines the extent</param>
        /// <param name="fields">Enumeration fo fields</param>
        /// <returns>true, if the form already contains a metaclass form</returns>
        public bool HasMetaClassFieldInForm(IExtent extent, IEnumerable<object> fields)
        {
            var typesWorkspace = _workspaceLogic.GetTypesWorkspace();

            return fields
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) ?? false);
        }
        
        /// <summary>
        /// Looks for the given field in the form and returns it, if it is available
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement? GetField(IElement form, string fieldName)
        {
            if (_DatenMeister._Forms._DetailForm.field != _DatenMeister._Forms._ListForm.field)
                throw new InvalidOperationException("Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");
            
            var fields = form.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            return fields
                .WhenPropertyHasValue(_DatenMeister._Forms._FieldData.name, fieldName)
                .OfType<IElement>()
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the enumeration of the detail forms which are in embedded into the tabs
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>Enumeration of the detail forms</returns>
        public static IEnumerable<IElement> GetDetailForms(IElement form)
        {
            foreach (var tab in form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab))
            {
                if (tab is IElement asElement 
                    && asElement.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__DetailForm) == true)
                {
                    yield return asElement;
                }
            }
        }

        /// <summary>
        /// Gets the enumeration of the detail forms which are in embedded into the tabs
        /// </summary>
        /// <param name="form">Form to be checked</param>
        /// <returns>Enumeration of the detail forms</returns>
        public static IEnumerable<IElement> GetListForms(IElement form)
        {
            foreach (var tab in form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab))
            {
                if (tab is IElement asElement 
                    && asElement.getMetaClass()?.equals(_DatenMeister.TheOne.Forms.__ListForm) == true)
                {
                    yield return asElement;
                }
            }
        }

        /// <summary>
        /// Gets the list tab listing the items of the properties
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="propertyName">Name of the property to which the propertyname shall belong</param>
        /// <returns>The found element</returns>
        public static IElement? GetListTabForPropertyName(IElement form, string propertyName)
        {
            if (_DatenMeister._Forms._ExtentForm.tab != _DatenMeister._Forms._DetailForm.tab)
                throw new InvalidOperationException("Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");

            var tabs = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab);

            foreach (var tab in tabs.OfType<IElement>())
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(tab.getMetaClass(), _DatenMeister.TheOne.Forms.__ListForm))
                {
                    var property = tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.property);
                    if (property == propertyName)
                    {
                        return tab;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the default view mode for a certain object by querying the view mode instances as
        /// given in the in the management workspace
        /// </summary>
        /// <param name="extent">Extent whose view mode is requested</param>
        /// <returns>Found element or null if not found</returns>
        public IElement? GetDefaultViewMode(IExtent? extent)
        {
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            
            var extentTypes = extent?.GetConfiguration()?.ExtentTypes;
            if (extentTypes != null)
            {
                foreach (var extentType in extentTypes)
                {
                    var result = managementWorkspace
                        .GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode)
                        .WhenPropertyHasValue(_DatenMeister._Forms._ViewMode.defaultExtentType, extentType)
                        .OfType<IElement>()
                        .FirstOrDefault();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return managementWorkspace
                .GetAllDescendentsOfType(_DatenMeister.TheOne.Forms.__ViewMode)
                .WhenPropertyHasValue(_DatenMeister._Forms._ViewMode.id, "Default")
                .OfType<IElement>()
                .FirstOrDefault();
        }
    }
}