#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {
        /// <summary>
        ///     Creates a detail form by considering the the information that is stored within
        ///     the given element. Dependent upon the creation mode, the form will be created
        ///     by using the metaclass or the set properties
        /// </summary>
        /// <param name="element"></param>
        /// <param name="creationMode"></param>
        /// <returns></returns>
        public IElement CreateDetailFormForItem(IObject element, FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            var cache = new FormCreatorCache();
            var createdForm = MofFactory.create(_DatenMeister.TheOne.Forms.__DetailForm);
            createdForm.set(_DatenMeister._Forms._DetailForm.name, "Item");

            FormMethods.AddToFormCreationProtocol(
                createdForm,
                "[FormCreator.CreateDetailFormForItem]: " + NamedElementMethods.GetName(element));

            if (!creationMode.AutomaticMetaClassField)
                createdForm.set(_DatenMeister._Forms._DetailForm.hideMetaInformation, true);

            AddFieldsToForm(createdForm, element, creationMode, cache);
            CleanupDetailForm(createdForm);
            return createdForm;
        }

        /// <summary>
        ///     Creates a detail form by using the metaclass of the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to which the form will be created</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created form for the metaclass</returns>
        public IElement CreateDetailFormByMetaClass(IElement? metaClass, FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            var createdForm = MofFactory.create(_DatenMeister.TheOne.Forms.__DetailForm);
            var name = NamedElementMethods.GetName(metaClass);
            createdForm.set(_DatenMeister._Forms._DetailForm.name, $"{name} - Detail");

            FormMethods.AddToFormCreationProtocol(
                createdForm,
                "[FormCreator.CreateDetailFormByMetaClass]: " + NamedElementMethods.GetName(metaClass));

            if (creationMode.AutomaticMetaClassField)
                createdForm.set(_DatenMeister._Forms._DetailForm.hideMetaInformation, true);

            if (!AddFieldsToFormByMetaClass(createdForm, metaClass, creationMode))
                createdForm.set(_DatenMeister._Forms._DetailForm.allowNewProperties, true);

            CleanupDetailForm(createdForm);
            return createdForm;
        }
        
        public void CleanupDetailForm(IElement detailForm)
        {
            SortFieldsByImportantProperties(detailForm);
            
        }
    }
}