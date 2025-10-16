using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

public class RemoveDuplicateDefaultTypes : FormFactoryBase, ITableFormFactory
{
    public void CreateTableForm(
        TableFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        foreach (var form in result.Forms)
        {
            var defaultNewTypesForElements =
                form.getOrDefault<IReflectiveCollection>(_Forms._TableForm.defaultTypesForNewElements);
            if (defaultNewTypesForElements == null)
            {
                // Nothing to do, when no default types are set
                return;
            }

            var handled = new List<IObject>();

            foreach (var element in defaultNewTypesForElements.OfType<IObject>().ToList())
            {
                var metaClass = element.getOrDefault<IObject>(_Forms._DefaultTypeForNewElement.metaClass);
                if (metaClass == null) continue;

                if (handled.Any(x => x.@equals(metaClass)))
                {
                    defaultNewTypesForElements.remove(element);
                }
                else
                {
                    handled.Add(metaClass);
                }
            }

            result.IsManaged = true;
        }
    }
}