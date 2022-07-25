using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Extent.Forms
{
    /// <summary>
    /// Modifies the form according to the Extent Type
    /// </summary>
    public class ExtentTypeFormModification : IFormModificationPlugin
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentSettings _extentSettings;

        public ExtentTypeFormModification(IWorkspaceLogic workspaceLogic, ExtentSettings extentSettings)
        {
            _workspaceLogic = workspaceLogic;
            _extentSettings = extentSettings;
        }

        /// <summary>
        /// Modifies the form by figuring out the extent type of the currently shown Collection Form.
        /// The extent type includes information about the supported metaclasses of the extent type themselves. 
        /// After that, they are going through each table form and adds the buttons to create new instances.
        ///
        /// The table form which is not associated to a classifier will receive creation buttons for all
        /// to the extenttype's associated classifiers except the ones where a table form is already existing.
        ///  
        /// </summary>
        /// <param name="context">Form Creation Context to be used</param>
        /// <param name="form">Form to be used</param>
        /// <returns>true, if the form has been modified. </returns>
        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            // Finds the extent type fitting to the extent to be shown
            var foundExtentType =
                _extentSettings.extentTypeSettings.FirstOrDefault(x => x.name == context.ExtentType);
            var mofFactory = new MofFactory(form);

            if (foundExtentType != null && context.FormType == _DatenMeister._Forms.___FormType.Collection)
            {
                var foundListMetaClasses = new List<IElement>();

                var tableForms = FormMethods.GetTableForms(form).ToList();
                // First, figure out which list forms we are having...
                foreach (var listForm in tableForms)
                {
                    var metaClass = listForm.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass);
                    foundListMetaClasses.Add(metaClass);
                }
                
                // Second, now create the action buttons for the tableform without classifier
                foreach (var listForm in tableForms)
                {
                    // Selects only the listform which do not have a classifier
                    if (listForm.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass) != null)
                    {
                        continue;
                    }
                    
                    foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                    {
                        var resolvedMetaClass =
                            _workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);

                        if (resolvedMetaClass == null)
                        {
                            continue;
                        }
                        
                        // The found metaclass already has a list form
                        if (foundListMetaClasses.Contains(resolvedMetaClass))
                        {
                            FormMethods.AddToFormCreationProtocol(listForm,
                                $"ExtentTypeFormsPlugin: Did not add {NamedElementMethods.GetName(resolvedMetaClass)} for ExtentType '{foundExtentType.name}' since it already got a listform");
                            continue;
                        }
                        
                        FormMethods.AddDefaultTypeForNewElement(form, resolvedMetaClass);

                        FormMethods.AddToFormCreationProtocol(listForm,
                            $"ExtentTypeFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}'");
                    }
                }
            }

            return false;
        }
    }
}