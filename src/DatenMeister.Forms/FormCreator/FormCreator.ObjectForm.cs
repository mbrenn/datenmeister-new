using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Forms.FormCreator
{
    public partial class FormCreator
    {

        public IElement CreateObjectFormForItem(IObject element, FormFactoryConfiguration configuration)
        {
            var detailForm = CreateRowFormForItem(element, configuration);

            return CreateObjectFormFromTabs(new MofFactory(detailForm), detailForm);
        }
        
        /// <summary>
        ///     Creates an extent form containing the subforms
        /// </summary>
        /// <param name="factory">The factory being used</param>
        /// <param name="tabsAsForms">The Forms which are converted to an extent form</param>
        /// <returns>The created extent</returns>
        public static IElement CreateObjectFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
        {
            factory ??= new MofFactory(tabsAsForms.First());
            var result = factory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            result.set(_DatenMeister._Forms._CollectionForm.tab, tabsAsForms);
            return result;
        }
        
    }
}