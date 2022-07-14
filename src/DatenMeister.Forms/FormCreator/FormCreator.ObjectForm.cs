﻿using System.IO;
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
        public IElement CreateObjectFormFromTabs(IFactory? factory, params IElement[] tabsAsForms)
        {
            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            result.set(_DatenMeister._Forms._CollectionForm.tab, tabsAsForms);
            return result;
        }

        /// <summary>
        /// Creates an object form by the definition of a metaclass
        /// </summary>
        /// <param name="metaClass">MetaClass to be handled</param>
        /// <param name="configuration">Configuration of the Form Factory</param>
        /// <returns>The returned Object Form</returns>
        public IElement CreateObjectFormForMetaClass(IElement metaClass, FormFactoryConfiguration configuration)
        {
            var result = MofFactory.create(_DatenMeister.TheOne.Forms.__ObjectForm);
            var rowForm = CreateRowFormByMetaClass(metaClass, configuration);
            result.set(_DatenMeister._Forms._CollectionForm.tab, new []{rowForm});
            return result;
        }
        
    }
}