using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class MetaClassElementField : IDetailField
    {
        public UIElement? CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            detailForm.CreateSeparator();

            var uriExtentText = ((value as IHasExtent)?.Extent as IUriExtent)?.contextURI() ?? string.Empty;
            var fullName = NamedElementMethods.GetFullName(value);
            detailForm.CreateRowForField("Extent:", uriExtentText, true);
            detailForm.CreateRowForField("Full Name:", fullName, true);
            detailForm.CreateRowForField("Url w/ ID:", (value as IElement)?.GetUri() ?? string.Empty, true);
            detailForm.CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

            var metaClass = (value as IElement)?.getMetaClass();

            var textField = new TextBlock
            {
                Text = metaClass == null ? "None" : NamedElementMethods.GetFullName(metaClass)
            };

            if (metaClass != null)
            {
                textField.TextDecorations = TextDecorations.Underline;
                textField.MouseDown += (sender, args) =>
                {
                    NavigatorForItems.NavigateToElementDetailView(
                        detailForm.NavigationHost,
                        metaClass);
                };
                
                textField.Foreground = SystemColors.HotTrackBrush;
                textField.Cursor = Cursors.Hand;
            }
            else
            {
                textField.FontStyle = FontStyles.Italic;
            }
            
            detailForm.CreateRowForField(
                new TextBlock { Text = "Meta Class:"},
                textField);

            return null;
        }

        public void CallSetAction(IObject element)
        {
            // Nothing to do
            return;
        }
    }
}