using System.Windows;
using System.Windows.Markup;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public class MetaClassElementField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            detailForm.CreateSeparator();

            var uriExtentText = ((value as IHasExtent)?.Extent as IUriExtent)?.contextURI() ?? string.Empty;
            var fullName = NamedElementMethods.GetFullName(value);
            detailForm.CreateRowForField("Extent:", uriExtentText, true);
            detailForm.CreateRowForField("Full Name:", fullName, true);
            detailForm.CreateRowForField("Url w/ ID:", (value as IElement)?.GetUri() ?? string.Empty, true);
            detailForm.CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

            var metaClass = (value as IElement)?.getMetaClass();
            detailForm.CreateRowForField(
                "Meta Class:",
                metaClass == null ? string.Empty : NamedElementMethods.GetFullName(metaClass),
                true);

            return null;
        }

        public void CallSetAction(IObject element)
        {
            // Nothing to do
            return;
        }
    }
}