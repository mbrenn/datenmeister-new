using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.ObjectForm;

/// <summary>
/// A form factory responsible for ensuring that an empty row form exists if no row forms
/// are currently available in the provided result. This class implements the logic to
/// manage object forms and adds an empty row form with default settings where necessary.
/// </summary>
public class AddEmptyRowFormInCaseItDoesNotExist : FormFactoryBase, IObjectFormFactory
{
    /// <summary>
    /// Creates an object form and ensures that an empty row form exists within the provided result,
    /// if no row forms are currently available. Adds the empty row form with default settings and
    /// updates the result form creation protocol accordingly.
    /// </summary>
    /// <param name="parameter">The parameters required for creating the object form, including element-specific data.</param>
    /// <param name="context">The context in which the form creation occurs, providing global and local scope.</param>
    /// <param name="result">The result of the form creation process, containing the form and any related metadata.</param>
    public void CreateObjectForm(
        ObjectFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultOneForm result)
    {
        if(result.Form == null) 
            return;
        
        var first = FormMethods.GetRowForms(result.Form).FirstOrDefault();
        if(first == null){
            var rowForm =context.Global.Factory.create(_Forms.TheOne.__RowForm);
            rowForm.set(_Forms._RowForm.name, "Empty");
            result.Form.AddCollectionItem(_Forms._ObjectForm.tab, rowForm);
            
            result.AddToFormCreationProtocol("Empty Row Form added because it does not exist");
            result.IsManaged = true;
        }
    }
}