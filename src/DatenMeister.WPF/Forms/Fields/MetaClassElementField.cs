using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Forms;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class MetaClassElementField : IDetailField
    {
        public UIElement? CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
        {
            detailForm.CreateSeparator();

            var fullName = NamedElementMethods.GetFullName(value);
            
            // Creates the ID block
            var uriExtentText = ((value as IHasExtent)?.Extent as IUriExtent)?.contextURI() ?? string.Empty;
            if (value is IHasId asHasId)
            {
                CreateIdElement(detailForm, asHasId);
            }

            // Creates information about extent, fullname and reference Urls
            detailForm.CreateRowForField("Extent:", uriExtentText, true);
            detailForm.CreateRowForField("Full Name:", fullName, true);
            detailForm.CreateRowForField("Url w/ ID:", (value as IElement)?.GetUri() ?? string.Empty, true);
            detailForm.CreateRowForField("Url w/Fullname:", $"{uriExtentText}?fn={fullName}", true);

            CreateMetaClassElement(detailForm, value);

            return null;
        }

        /// <summary>
        /// Creates the line showing the id of the current element and allows the user to modify the id
        /// </summary>
        /// <param name="detailForm">Detail Form Control which is the host. </param>
        /// <param name="asHasId">The element containing the id</param>
        private static void CreateIdElement(DetailFormControl detailForm, IHasId asHasId)
        {
            var idTextBlock = detailForm.CreateRowForField("Id:", asHasId.Id ?? "None", true);

            if (asHasId is ICanSetId asSetId)
            {
                UnderlineForLink(idTextBlock);
                idTextBlock.MouseDown += async (x, y) =>
                {
                    var formsPlugin = GiveMe.Scope.Resolve<FormsPlugin>();
                    var form = formsPlugin.GetInternalFormExtent().element("#CommonForms.ChangeId");

                    var element = InMemoryObject.CreateEmpty();
                    element.set("oldId", asHasId.Id);
                    element.set("newId", asHasId.Id);

                    var result = await NavigatorForItems.NavigateToElementDetailView(detailForm.NavigationHost,
                        new NavigateToItemConfig
                        {
                            Form = new FormDefinition(form),
                            DetailElement = element
                        });

                    if (result.Result == NavigationResult.Saved)
                    {
                        var newId = result.DetailElement.getOrDefault<string>("newId");
                        if (newId != null && !string.IsNullOrEmpty(newId))
                        {
                            try
                            {
                                asSetId.Id = newId;
                            }
                            catch (InvalidOperationException exc)
                            {
                                MessageBox.Show(exc.Message);
                            }
                            
                            detailForm.UpdateForm();
                        }
                        else
                        {
                            MessageBox.Show("An empty ID is not valid");
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Creates the line containing the metaclass of the element and allowing the user to modify the metaclass 
        /// </summary>
        /// <param name="detailForm">Detailform to be used</param>
        /// <param name="value">Value of the element</param>
        private static void CreateMetaClassElement(DetailFormControl detailForm, IObject value)
        {
            // Creates the information about the metaclasse
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
            
            if (value is IElementSetMetaClass asSet)
            {
                textField.Inlines.Add(" (");
                var change = new Run("Change")
                {
                    Foreground = SystemColors.HotTrackBrush,
                    Cursor = Cursors.Hand
                };
                UnderlineForLink(change);
                change.MouseDown += async (x, y) =>
                {
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
            }

            detailForm.CreateRowForField(
                new TextBlock {Text = "Meta Class:"},
                textField);
        }

        private static void UnderlineForLink(DependencyObject idTextBlock)
        {
            if (idTextBlock is TextBlock textBlock)
            {
                textBlock.TextDecorations = TextDecorations.Underline;
                textBlock.Foreground = SystemColors.HotTrackBrush;
                textBlock.Cursor = Cursors.Hand;
            }
            else if (idTextBlock is Run run)
            {
                run.TextDecorations = TextDecorations.Underline;
                run.Foreground = SystemColors.HotTrackBrush;
                run.Cursor = Cursors.Hand;
            }
        }

        public void CallSetAction(IObject element)
        {
            // Nothing to do
        }
    }
}