using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Extent.Forms;

/// <summary>
///     Defines the default type modification plugin.
/// It finds the 'packagedElement' of a package and adds all preferred type as being mentioned
/// in the package instance itself
/// </summary>
public class PackageFormModificationPlugin : INewObjectFormFactory
{
    private static void AddPreferredTypes(
        IObject form,
        IFactory factory,
        IReflectiveCollection? preferredTypes,
        IReflectiveCollection defaultTypes)
    {
        if (preferredTypes != null)
            foreach (var preferredType in preferredTypes.OfType<IElement>())
                AddPreferredType(form, factory, preferredType, defaultTypes);
    }

    private static void AddPreferredType(
        IObject form,
        IFactory factory,
        IElement preferredType,
        IReflectiveCollection defaultTypes)
    {
        if (preferredType.getMetaClass()?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
        {
            var defaultType = factory.create(_Forms.TheOne.__DefaultTypeForNewElement);
            defaultType.set(_Forms._DefaultTypeForNewElement.name,
                NamedElementMethods.GetName(preferredType));
            defaultType.set(_Forms._DefaultTypeForNewElement.metaClass, preferredType);
            defaultTypes.add(defaultType);

            FormMethods.AddToFormCreationProtocol(
                form,
                "[PackageFormModificationPlugin]: Add DefaultType by preferred Types" +
                NamedElementMethods.GetName(defaultType));
        }
    }

    public void CreateObjectForm(ObjectFormFactoryParameter parameter, NewFormCreationContext context, FormCreationResult result)
    {
        var element = parameter.Element;
        if (element == null)
            return;
        
        if (result.Form == null)
        {
            throw new InvalidOperationException("Form is null");
        }

        if ((element as IElement)?.metaclass?.Equals(_CommonTypes.TheOne.Default.__Package) == true)
        {
            var tabPackagedElement =
                FormMethods.GetTableFormForPropertyName(result.Form,
                    _CommonTypes._Default._Package.packagedElement);

            if (tabPackagedElement != null)
            {
                var defaultTypes =
                    tabPackagedElement.get<IReflectiveCollection>(_Forms._TableForm
                        .defaultTypesForNewElements);

                // Checks the preferred types
                var preferredTypes =
                    element.getOrDefault<IReflectiveCollection>(
                        _CommonTypes._Default._Package.preferredType);

                AddPreferredTypes(result.Form, context.Global.Factory, preferredTypes, defaultTypes);

                // Checks the preferred package. 
                // If a preferred package is set, then all containing classes will be added
                var preferredPackages =
                    element.getOrDefault<IReflectiveCollection>(
                        _CommonTypes._Default._Package.preferredPackage);

                if (preferredPackages != null)
                {
                    foreach (var preferredPackage in preferredPackages.OfType<IElement>())
                    {
                        var preferredTypes2 = PackageMethods.GetPackagedObjects(preferredPackage);
                        AddPreferredTypes(result.Form, context.Global.Factory, preferredTypes2, defaultTypes);
                    }
                }
            }

            result.IsManaged = true;
        }
    }

    public void CreateObjectFormForMetaClass(IElement? metaClass, NewFormCreationContext context,
        FormCreationResult result)
    {
    }
}