using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Tags;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Defines some helper methods to add certain standard view extensions to the
    /// nevigation guests
    /// </summary>
    public static class ViewExtensionHelper
    {
        /// <summary>
        /// Gets a generic button which allows the creation of a new item
        /// by the user
        /// </summary>
        /// <param name="navigationHost">Navigation host being used</param>
        /// <param name="metaclass">Meta Class being used for the creation</param>
        /// <param name="collection">The collection to which the new element will be created</param>
        /// <returns>The created </returns>
        public static ViewExtension GetCreateButtonForMetaClass(
            INavigationHost navigationHost,
            IElement metaclass,
            IReflectiveCollection collection)
        {
            var typeName = metaclass.get(_UML._CommonStructure._NamedElement.name);

            return new GenericButtonDefinition(
                $"New {typeName}", async () =>
                {
                    await NavigatorForItems.NavigateToNewItemForCollection(
                        navigationHost,
                        collection,
                        metaclass);
                })
            {
                Tag = new TagCreateMetaClass(metaclass)
            };
        }
    }
}