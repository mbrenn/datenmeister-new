using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
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

        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            // Finds the extent type fitting to the extent to be shown
            var foundExtentType =
                _extentSettings.extentTypeSettings.FirstOrDefault(x => x.name == context.ExtentType);
            var mofFactory = new MofFactory(form);

            if (foundExtentType != null && context.FormType == _DatenMeister._Forms.___FormType.TreeItemExtent)
            {
                foreach (var listForm in FormMethods.GetListForms(form))
                {
                    // Selects only the listform which do not have a classifier
                    if (listForm.getOrDefault<IElement>(_DatenMeister._Forms._ListForm.metaClass) != null)
                    {
                        continue;
                    }

                    foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                    {
                        var resolvedMetaClass =
                            _workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);
                        var defaultTypesForNewElements =
                            listForm.get<IReflectiveSequence>(
                                _DatenMeister._Forms._ListForm
                                    .defaultTypesForNewElements);

                        var defaultType = mofFactory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                        defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, resolvedMetaClass);
                        defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                            NamedElementMethods.GetName(resolvedMetaClass));
                        defaultTypesForNewElements.add(defaultType);

                        FormMethods.AddToFormCreationProtocol(listForm,
                            $"TypesFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}'");
                    }
                }
            }

            return false;
        }
    }
}