#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        /// Creates a detail form by considering the the information that is stored within
        /// the given element. Dependent upon the creation mode, the form will be created
        /// by using the metaclass or the set properties
        /// </summary>
        /// <param name="element"></param>
        /// <param name="creationMode"></param>
        /// <returns></returns>
        public IElement CreateDetailForm(IObject element, CreationMode creationMode = CreationMode.All)
        {
            var cache = new FormCreatorCache();
            var createdForm = _factory.create(_DatenMeister.TheOne.Forms.__DetailForm);
            createdForm.set(_DatenMeister._Forms._DetailForm.name, "Item");

            if (creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                createdForm.set(_DatenMeister._Forms._DetailForm.hideMetaInformation, true);
            }

            AddToForm(createdForm, element, creationMode, cache);
            return createdForm;
        }

        /// <summary>
        /// Creates a detail form by using the metaclass of the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to which the form will be created</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created form for the metaclass</returns>
        public IElement CreateDetailFormByMetaClass(IElement metaClass, CreationMode creationMode = CreationMode.All)
        {
            var createdForm = _factory.create(_DatenMeister.TheOne.Forms.__DetailForm);
            var name = NamedElementMethods.GetName(metaClass);
            createdForm.set(_DatenMeister._Forms._DetailForm.name, $"{name} - Detail");

            if (creationMode.HasFlag(CreationMode.AddMetaClass))
            {
                createdForm.set(_DatenMeister._Forms._DetailForm.hideMetaInformation, true);
            }

            if (!AddToFormByMetaclass(createdForm, metaClass, creationMode))
            {
                createdForm.set(_DatenMeister._Forms._DetailForm.allowNewProperties, true);
            }

            return createdForm;
        }
    }
}