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
        public IElement CreateRowFormForItem(IObject element, FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            var cache = new FormCreatorCache();
            var createdForm = MofFactory.create(_DatenMeister.TheOne.Forms.__RowForm);
            createdForm.set(_DatenMeister._Forms._RowForm.name, "Item");
            createdForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                createdForm,
                "[FormCreator.CreateRowFormForItem]: " + NamedElementMethods.GetName(element));

            if (!creationMode.AutomaticMetaClassField)
                createdForm.set(_DatenMeister._Forms._RowForm.hideMetaInformation, true);

            AddFieldsToForm(createdForm, element, creationMode, cache);
            CleanupRowForm(createdForm);
            return createdForm;
        }

        /// <summary>
        ///     Creates a detail form by using the metaclass of the given element
        /// </summary>
        /// <param name="metaClass">Metaclass to which the form will be created</param>
        /// <param name="creationMode">The creation mode being used</param>
        /// <returns>The created form for the metaclass</returns>
        public IElement CreateRowFormByMetaClass(IElement? metaClass, FormFactoryConfiguration? creationMode = null)
        {
            creationMode ??= new FormFactoryConfiguration();
            var createdForm = MofFactory.create(_DatenMeister.TheOne.Forms.__RowForm);
            var name = NamedElementMethods.GetName(metaClass);
            createdForm.set(_DatenMeister._Forms._RowForm.name, $"{name} - Detail");
            createdForm.set(_DatenMeister._Forms._RowForm.isAutoGenerated, true);

            FormMethods.AddToFormCreationProtocol(
                createdForm,
                "[FormCreator.CreateDetailFormByMetaClass]: " + NamedElementMethods.GetName(metaClass));

            if (creationMode.AutomaticMetaClassField)
                createdForm.set(_DatenMeister._Forms._RowForm.hideMetaInformation, true);

            if (!AddFieldsToRowOrTableFormByMetaClass(createdForm, metaClass, creationMode))
                createdForm.set(_DatenMeister._Forms._RowForm.allowNewProperties, true);

            CleanupRowForm(createdForm);
            return createdForm;
        }
        
        public void CleanupRowForm(IElement rowForm)
        {
            SortFieldsByImportantProperties(rowForm);
        }
    }
}