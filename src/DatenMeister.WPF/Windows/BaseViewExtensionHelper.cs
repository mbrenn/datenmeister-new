using System;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base.ViewExtensions;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// This helper stores the possible scopes for which a ribbon or a
    /// menu shall be created
    /// </summary>
    public class BaseViewExtensionHelper
    {
        public IExtent Extent { get; set; }
        
        public IReflectiveCollection Collection { get; set; }
        
        public IObject Item { get; set; }

        protected Action CreateClickMethod(RibbonButtonDefinition definition)
        {
            // Defines the clickmethod by the definition
            Action clickMethod = null;
            if (definition is ApplicationMenuButtonDefinition applicationMenuButtonDefinition)
            {
                clickMethod = applicationMenuButtonDefinition.OnPressed;
            }

            if (Extent != null && definition is ExtentMenuButtonDefinition extentMenuButtonDefinition)
            {
                clickMethod = () => extentMenuButtonDefinition.OnPressed(Extent);
            }

            if (Collection != null && definition is CollectionMenuButtonDefinition collectionMenuButtonDefinition)
            {
                clickMethod = () => collectionMenuButtonDefinition.OnPressed(Collection);
            }

            if (Item != null && definition is ItemMenuButtonDefinition itemMenuButtonDefinition)
            {
                clickMethod = () => itemMenuButtonDefinition.OnPressed(Item);
            }

            return clickMethod;
        }
    }
}