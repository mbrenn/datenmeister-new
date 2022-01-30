﻿using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Extent.Forms
{
    /// <summary>
    ///     Defines the default type modification pluging
    /// </summary>
    public class PackageFormModificationPlugin : IFormModificationPlugin
    {
        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            if (context.MetaClass?.Equals(_DatenMeister.TheOne.CommonTypes.Default.__Package) == true
                && context.FormType == _DatenMeister._Forms.___FormType.Detail
                && context.ParentPropertyName == string.Empty
                && context.DetailElement != null)
            {
                var tabPackagedElement =
                    FormMethods.GetListTabForPropertyName(form,
                        _DatenMeister._CommonTypes._Default._Package.packagedElement);

                if (tabPackagedElement != null)
                {
                    var factory = new MofFactory(form);

                    var defaultTypes =
                        tabPackagedElement.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm
                            .defaultTypesForNewElements);

                    // Checks the preferred types
                    var preferredTypes =
                        context.DetailElement.getOrDefault<IReflectiveCollection>(
                            _DatenMeister._CommonTypes._Default._Package.preferredType);

                    AddPreferredTypes(form, factory, preferredTypes, defaultTypes);

                    // Checks the preferred package. 
                    // If a preferred package is set, then all containing classes will be added
                    var preferredPackages =
                        context.DetailElement.getOrDefault<IReflectiveCollection>(
                            _DatenMeister._CommonTypes._Default._Package.preferredPackage);

                    if (preferredPackages != null)
                        foreach (var preferredPackage in preferredPackages.OfType<IElement>())
                        {
                            var preferredTypes2 = PackageMethods.GetPackagedObjects(preferredPackage);
                            AddPreferredTypes(form, factory, preferredTypes2, defaultTypes);
                        }
                }

                return true;
            }

            return false;
        }

        private static void AddPreferredTypes(
            IObject form,
            MofFactory factory,
            IReflectiveCollection? preferredTypes,
            IReflectiveCollection defaultTypes)
        {
            if (preferredTypes != null)
                foreach (var preferredType in preferredTypes.OfType<IElement>())
                    AddPreferredType(form, factory, preferredType, defaultTypes);
        }

        private static void AddPreferredType(
            IObject form,
            MofFactory factory,
            IElement preferredType,
            IReflectiveCollection defaultTypes)
        {
            if (preferredType.getMetaClass()?.Equals(_UML.TheOne.StructuredClassifiers.__Class) == true)
            {
                var defaultType = factory.create(_DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
                defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.name,
                    NamedElementMethods.GetName(preferredType));
                defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, preferredType);
                defaultTypes.add(defaultType);

                FormMethods.AddToFormCreationProtocol(
                    form,
                    "[PackageFormModificationPlugin]: Add DefaultType " + NamedElementMethods.GetName(defaultType));
            }
        }
    }
}