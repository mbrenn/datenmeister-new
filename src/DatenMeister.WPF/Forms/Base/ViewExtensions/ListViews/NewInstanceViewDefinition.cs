#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base.ViewExtensions.ListViews
{
    /// <summary>
    /// Defines a view definition which allows the user to create a new instance
    /// of the given type by clicking upon the connected UI element. 
    /// </summary>
    public class NewInstanceViewDefinition : ViewExtension
    {
        public IElement MetaClass { get; set; }

        public NewInstanceViewDefinition(IElement metaClass)
        {
            MetaClass = metaClass;
        }
    }
}