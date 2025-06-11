using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms.Helper;

public static class CollectionFormHelper
{
    
    /// <summary>
    ///     Creates an extent form containing the subforms
    /// </summary>
    /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
    /// <returns>The created extent</returns>
    public static IElement CreateCollectionFormFromTabs(params IElement[] tabsAsForms)
    {
        var factory = new MofFactory(tabsAsForms.First());
        return CreateCollectionFormFromTabs(factory, tabsAsForms);
    }

    /// <summary>
    ///     Creates an extent form containing the subforms
    /// </summary>
    /// <param name="factory">The factory being used</param>
    /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
    /// <returns>The created extent</returns>
    public static IElement CreateCollectionFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
    {
        factory ??= new MofFactory(tabsAsForms.First());
        var result = factory.create(_Forms.TheOne.__CollectionForm);
        result.set(_Forms._CollectionForm.tab, tabsAsForms);
        return result;
    }
}