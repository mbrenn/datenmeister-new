using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Forms.RowForm;

public class AddMetaClassField : FormFactoryBase, IRowFormFactory
{
    public void CreateRowForm(
        RowFormFactoryParameter parameter,
        FormCreationContext context,
        FormCreationResultMultipleForms result)
    {
        if (result.Forms.Count == 0) 
            return;
        
        // First, check if we have already a form in one of the forms
        // Really first, by cache
        if (context.LocalScopeStorage.TryGet<FormCreatorCache>()?.MetaClassAlreadyAdded == true)
            return;
        
        // Then manually
        var hasAnyMetaClass = result.Forms.Any(x =>
            x.get<IReflectiveCollection>(_Forms._RowForm.field).OfType<IElement>()
                .Any(x => x.getMetaClass()?.equals(_Forms.TheOne.__MetaClassElementFieldData) == true));
        if (hasAnyMetaClass)
            return;
        
        // Ok, there is no metaclass, so we add it on the first form
        var cache = context.LocalScopeStorage.Get<FormCreatorCache>();
        cache.MetaClassAlreadyAdded = true;

        var fieldData = context.Global.Factory.create(_Forms.TheOne.__MetaClassElementFieldData);
        result.Forms.First().AddCollectionItem(
            _Forms._RowForm.field, fieldData);
        
        result.IsManaged = true;
    }    
}