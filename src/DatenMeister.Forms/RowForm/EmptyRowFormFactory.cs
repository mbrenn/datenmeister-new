using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;

namespace DatenMeister.Forms.RowForm;

public class EmptyRowFormFactory : IRowFormFactory
{
    public void CreateRowForm(RowFormFactoryParameter parameter, FormCreationContext context, FormCreationResult result)
    {
        result.Form ??= context.Global.Factory.create(_Forms.TheOne.__RowForm);
        result.IsManaged = true;
    }
}