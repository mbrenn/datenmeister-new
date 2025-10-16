using System.Collections;
using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;
using DatenMeister.Forms.TableForms;

namespace DatenMeister.Forms.ObjectForm;

public class ValidateObjectOrCollectionForm : FormFactoryBase, IObjectFormFactory, ICollectionFormFactory
{
    private static readonly ILogger Logger = new ClassLogger(typeof(ValidateTableOrRowForm));

    private static void Validate(FormFactoryParameterBase parameter, FormCreationContext context,
        FormCreationResultOneForm result)
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

        var form = result.Form;

        Debug.Assert(_Forms._CollectionForm.tab == _Forms._ObjectForm.tab);

        var tabs = form.getOrDefault<IReflectiveCollection>(_Forms._CollectionForm.tab);
        if (tabs != null)
        {       
            var clonedContext = context.Clone();
            var validator = new ValidateTableOrRowForm();
            
            
            foreach (var tab in tabs.OfType<IElement>())
            {
                var formCreationResult = new FormCreationResultMultipleForms
                {
                    IsMainContentCreated = true
                };
                
                formCreationResult.Forms.Add(tab);
                
                validator.CreateTableForm(
                    new TableFormFactoryParameter(),
                    clonedContext,
                    formCreationResult);
                
                if (clonedContext.LocalScopeStorage.Get<ValidationResult>().IsInvalid)
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

    public void CreateObjectForm(ObjectFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultOneForm result)
    {
        Validate(parameter, context, result);
    }

    public void CreateCollectionForm(CollectionFormFactoryParameter parameter, FormCreationContext context,
        FormCreationResultOneForm result)
    {
        Validate(parameter, context, result);
    }
}