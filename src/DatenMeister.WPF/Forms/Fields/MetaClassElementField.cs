using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
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
            detailForm.CreateRowForField("Url w/Fullname:", $"{uriExtentText}?fn={fullName}", true);

            var metaClass = (value as IElement)?.getMetaClass();

            var textField = new TextBlock();
            var runName = new Run(metaClass == null ? "None" : NamedElementMethods.GetFullName(metaClass));
            if (metaClass != null)
            {
                runName.TextDecorations = TextDecorations.Underline;
                runName.MouseDown += (sender, args) =>
                {
                    if (metaClass == null)
                    {
                        MessageBox.Show("The 'metaClass' is not found.");
                    }
                    else
                    {
                        _ = NavigatorForItems.NavigateToElementDetailView(
                            detailForm.NavigationHost,
                            metaClass);
                    }
                };
                
                runName.Foreground = SystemColors.HotTrackBrush;
                runName.Cursor = Cursors.Hand;
            }
            else
            {
                runName.FontStyle = FontStyles.Italic;
            }
            
            textField.Inlines.Add(runName);
            textField.Inlines.Add(" (");
            var change = new Run("Change")
            {
                TextDecorations = TextDecorations.Underline,
                Foreground = SystemColors.HotTrackBrush,
                Cursor = Cursors.Hand
            };
            change.MouseDown += async (x, y) =>
            {
                if (!(value is IElementSetMetaClass asSet))
                {
                    MessageBox.Show("Metaclass cannot be set since object does not allow setting");
                    return;
                }

                if (await NavigatorForDialogs.Locate(
                    detailForm.NavigationHost,
                    WorkspaceNames.WorkspaceTypes,
                    WorkspaceNames.UriExtentUserTypes) is IElement result)
                {
                    asSet.SetMetaClass(result);
                }
                else
                {
                    MessageBox.Show("No MetaClass was selected");
                }
                
                detailForm.UpdateForm();
            };
            textField.Inlines.Add(change);
            textField.Inlines.Add(")");
            
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