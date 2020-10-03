using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Models.EMOF;
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
            if (context.MetaClass?.Equals(_DatenMeister.TheOne.CommonTypes.Default.__Package) == true
                && context.FormType == FormType.TreeItemDetail
                && context.ParentPropertyName == string.Empty
                && context.DetailElement != null)
            {
                var tabPackagedElement =
                    FormMethods.GetListTabForPropertyName(form, _DatenMeister._CommonTypes._Default._Package.packagedElement);

                if (tabPackagedElement != null)
                {
                    var factory = new MofFactory(form);
                    
                    var defaultTypes =
                        tabPackagedElement.get<IReflectiveCollection>(_FormAndFields._ListForm.defaultTypesForNewElements);

                    // Checks the preferred types
                    var preferredTypes =
                        context.DetailElement.getOrDefault<IReflectiveCollection>(
                            _DatenMeister._CommonTypes._Default._Package.preferredType);

                    AddPreferredTypes(factory, preferredTypes, defaultTypes);
                    
                    // Checks the preferred package. 
                    // If a preferred package is set, then all containing classes will be added
                    var preferredPackages =
                        context.DetailElement.getOrDefault<IReflectiveCollection>(
                            _DatenMeister._CommonTypes._Default._Package.preferredPackage);

                    if (preferredPackages != null)
                    {
                        foreach (var preferredPackage in preferredPackages.OfType<IElement>())
                        {
                            var preferredTypes2 = PackageMethods.GetPackagedObjects(preferredPackage);
                            AddPreferredTypes(factory, preferredTypes2, defaultTypes);
                        }
                    }
                }
            }
        }

        private static void AddPreferredTypes(
            MofFactory factory,
            IReflectiveCollection? preferredTypes,
            IReflectiveCollection defaultTypes)
        {
            if (preferredTypes != null)
            {
                foreach (var preferredType in preferredTypes.OfType<IElement>())
                {
                    AddPreferredType(factory, preferredType, defaultTypes);
                }
            }
        }

        private static void AddPreferredType(MofFactory factory, IElement preferredType, IReflectiveCollection defaultTypes)
        {
            if (preferredType.getMetaClass()?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
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