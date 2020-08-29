using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Forms.FormModifications;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.DefaultTypes
{
    /// <summary>
    /// Defines the default type modification pluging
    /// </summary>
    public class DefaultTypesFormModificationPlugin : IFormModificationPlugin
    {
        public void ModifyForm(FormCreationContext context, IElement form)
        {
            if (context.MetaClass?.Equals(_CommonTypes.TheOne.Default.__Package) == true
                && context.FormType == FormType.TreeItemDetail
                && context.ParentPropertyName == string.Empty
                && context.DetailElement != null)
            {
                var tabPackagedElement =
                    FormMethods.GetListTabForPropertyName(form, _CommonTypes._Default._Package.packagedElement);

                if (tabPackagedElement != null)
                {
                    var defaultTypes =
                        tabPackagedElement.get<IReflectiveCollection>(_FormAndFields._ListForm.defaultTypesForNewElements);

                    var preferredTypes =
                        context.DetailElement.getOrDefault<IReflectiveCollection>(
                            _CommonTypes._Default._Package.preferredType);

                    if (preferredTypes != null)
                    {
                        var factory = new MofFactory(form);
                        foreach (var preferredType in preferredTypes.OfType<IElement>())
                        {
                            var defaultType = factory.create(_FormAndFields.TheOne.__DefaultTypeForNewElement);
                            defaultType.set(_FormAndFields._DefaultTypeForNewElement.name,
                                NamedElementMethods.GetName(preferredType));
                            defaultType.set(_FormAndFields._DefaultTypeForNewElement.metaClass, preferredType);
                            defaultTypes.add(defaultType);
                        }
                    }
                }
            }
        }
    }
}