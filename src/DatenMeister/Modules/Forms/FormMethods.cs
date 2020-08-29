using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms
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
            var randomGuid = Guid.NewGuid();
            var set = new HashSet<string>();
            foreach (var field in fields.OfType<IObject>())
            {
                var preName = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                var isAttached = field.getOrDefault<bool>(_FormAndFields._FieldData.isAttached);
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
            var formAndFields = form.GetExtentOf()?.GetWorkspace()?.GetFromMetaWorkspace<_FormAndFields>()
                                ?? _FormAndFields.TheOne;
            return form
                .get<IReflectiveCollection>(_FormAndFields._DetailForm.field)
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.@equals(formAndFields.__MetaClassElementFieldData) ?? false);
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
            var formAndFields = typesWorkspace.Get<_FormAndFields>()
                                ?? _FormAndFields.TheOne;

            return fields
                .OfType<IElement>()
                .Any(x => x.getMetaClass()?.@equals(formAndFields.__MetaClassElementFieldData) ?? false);
        }
        
        /// <summary>
        /// Looks for the given field in the form and returns it, if it is available
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="fieldName">Name of the field</param>
        /// <returns>The found element or null, if not found</returns>
        public static IElement? GetField(IElement form, string fieldName)
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
        /// Gets the list tab listing the items of the properties
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        /// <param name="propertyName">Name of the property to which the propertyname shall belong</param>
        /// <returns>The found element</returns>
        public static IElement? GetListTabForPropertyName(IElement form, string propertyName)
        {
            if (_FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab)
                throw new InvalidOperationException("Something ugly happened here: _FormAndFields._ExtentForm.tab != _FormAndFields._DetailForm.tab");

            var tabs = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            var formAndFields = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();
            if (formAndFields == null) throw new InvalidOperationException("_FormAndFields were not found");

            foreach (var tab in tabs.OfType<IElement>())
            {
                if (ClassifierMethods.IsSpecializedClassifierOf(tab.getMetaClass(), formAndFields.__ListForm))
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

        /// <summary>
        /// Gets the default view mode for a certain object by querying the view mode instances as
        /// given in the in the management workspace
        /// </summary>
        /// <param name="extent">Extent whose view mode is requested</param>
        /// <returns>Found element or null if not found</returns>
        public IElement? GetDefaultViewMode(IExtent? extent)
        {
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            var formAndFields = managementWorkspace.GetFromMetaWorkspace<_FormAndFields>()
                                ?? throw new InvalidOperationException("_FormAndFields are empty");
            
            var extentTypes = extent?.GetConfiguration()?.ExtentTypes;
            if (extentTypes != null)
            {
                foreach (var extentType in extentTypes)
                {
                    var result = managementWorkspace
                        .GetAllDescendentsOfType(formAndFields.__ViewMode)
                        .WhenPropertyHasValue(_FormAndFields._ViewMode.defaultExtentType, extentType)
                        .OfType<IElement>()
                        .FirstOrDefault();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return managementWorkspace
                .GetAllDescendentsOfType(formAndFields.__ViewMode)
                .WhenPropertyHasValue(_FormAndFields._ViewMode.id, "Default")
                .OfType<IElement>()
                .FirstOrDefault();
        }
    }
}