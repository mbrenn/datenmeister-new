using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

public class SortFieldsByImportantProperties : FormFactoryBase, ITableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (!context.IsInExtensionCreationMode())
            return;
        
        foreach (var form in result.Forms)
        {
            var fields = form?.getOrDefault<IReflectiveSequence>(_Forms._TableForm.field);
            if (fields == null)
                return;

            var fieldsAsList = fields.OfType<IElement>().ToList();

            // Check if the name is within the list, if yes, push it to the front
            var fieldName = fieldsAsList.FirstOrDefault(x =>
                x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) ==
                _UML._CommonStructure._NamedElement.name);

            if (fieldName != null)
            {
                fields.remove(fieldName);
                fields.add(0, fieldName);

                result.AddToFormCreationProtocol(
                    "[FormCreator.SortFieldsByImportantProperties]: Field 'name' was put up-front");

                result.IsManaged = true;
            }

            // Performs a resetting of all properties
            // form.set(_DatenMeister._Forms._TableForm.field, fieldsAsList);
        }
    }
}
