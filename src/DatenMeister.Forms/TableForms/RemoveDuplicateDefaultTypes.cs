using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.TableForms;

public class RemoveDuplicateDefaultTypes : ITableFormFactory
{
    public void CreateTableForm(TableFormFactoryParameter parameter, FormCreationContext context, FormCreationResult result)
    {
        var form = result.Form;
        if (form == null)
            return;
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