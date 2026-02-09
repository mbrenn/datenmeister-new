using System.Collections;
using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.TableForms;

public class ValidateTableOrRowForm : FormFactoryBase, ITableFormFactory
{
    private static readonly ILogger Logger = new ClassLogger(typeof(ValidateTableOrRowForm));

    public void CreateTableForm(
        TableFormFactoryParameter parameter, 
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        var validationResult = context.LocalScopeStorage.Get<ValidationResult>(false);
        if (!result.IsMainContentCreated)
        {
            // Nothing needs to be checked
            return;
        }

        if (validationResult.IsInvalid)
        {
            // We do not need to check anything, because it is already invalid
        }

        foreach (var form in result.Forms)
        {
            // Check that the namings are the same, this is just a static check to avoid compile errors
            Debug.Assert(_Forms._RowForm.field == _Forms._TableForm.field);

            var fields = form.getOrDefault<IReflectiveCollection>(_Forms._RowForm.field);
            if (fields != null)
            {
                if (!ValidateFields(fields))
                {
                    validationResult.IsInvalid = true;
                }
            }
        }
    }


    /// <summary>
    ///     Checks, if there is a duplicated name of the fields
    /// </summary>
    /// <param name="fields">Fields to be enumerated</param>
    /// <returns>true, if there are no duplications</returns>
    private static bool ValidateFields(IEnumerable fields)
    {
        // Creates a random GUID to establish a separate namespace for attached fields
        var randomGuid = Guid.NewGuid();

        // Now go through the hash set
        var set = new HashSet<string>();
        foreach (var field in fields.OfType<IObject>())
        {
            var preName = field.getOrDefault<string>(_Forms._FieldData.name);
            var isAttached = field.getOrDefault<bool>(_Forms._FieldData.isAttached);
            var name = isAttached ? randomGuid + preName : preName;

            if (set.Contains(name) && !string.IsNullOrEmpty(name))
            {
                Logger.Warn($"Field '{name}' is included twice. Validation of form failed");
                return false;
            }

            set.Add(name);
        }

        return true;
    }

    public static bool ValidateForm(IElement form)
    {
        var context = new FormCreationContext
        {
            Global = new FormCreationContext.GlobalContext
            {
                Factory = new MofFactory(form),
                FactoryForForms = new MofFactory(form)
            }
        };

        var result = new FormCreationResultMultipleForms
        {
            IsMainContentCreated = true
        };
        
        result.Forms.Add(form);

        var validateTableOrRowForm = new ValidateTableOrRowForm();
        validateTableOrRowForm.CreateTableForm(new TableFormFactoryParameter(), context, result);

        return !context.LocalScopeStorage.Get<ValidationResult>().IsInvalid;
    }
}