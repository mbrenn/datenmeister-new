using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Uml.Helper;
using DatenMeister.UserInteractions;
using DatenMeisterWPF.Command;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Forms.Detail.Fields;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl, INavigationGuest, IHasSelectedItems
    {
        private int _fieldCount;

        private bool? _hideViewSelection;

        public DetailFormControl()
        {
            InitializeComponent();
            Loaded += DetailFormControl_Loaded;
        }

        /// <summary>
        ///     Gets the detailed element, whose content is shown in the dialog
        /// </summary>
        public IObject DetailElement { get; set; }

        /// <summary>
        ///     Defines the form definition being used in the detail for
        /// </summary>
        public IElement EffectiveForm { get; private set; }

        /// <summary>
        ///     Gets or sets the view definition
        /// </summary>
        public ViewDefinition ViewDefinition { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether new properties may be added by the user to the element
        /// </summary>
        public bool AllowNewProperties { get; set; }

        public IElement AttachedElement { get; private set; }

        /// <summary>
        ///     Gets the default size
        /// </summary>
        public Size DefaultSize => new Size(
            DotNetHelper.AsDouble(EffectiveForm?.get(_FormAndFields._Form.defaultWidth)),
            DotNetHelper.AsDouble(EffectiveForm?.get(_FormAndFields._Form.defaultHeight))
        );

        public List<IDetailField> ItemFields { get; } = new List<IDetailField>();

        public List<IDetailField> AttachedItemFields { get; } = new List<IDetailField>();

        public IObject GetSelectedItem()
        {
            return DetailElement;
        }

        public IEnumerable<IObject> GetSelectedItems()
        {
            yield return DetailElement;
        }

        /// <summary>
        ///     Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            yield return new RibbonButtonDefinition(
                "View-Configuration",
                ViewConfig,
                null,
                NavigationCategories.File + ".Views");

            yield return new RibbonButtonDefinition(
                "Copy",
                CopyContent,
                null,
                NavigationCategories.File + ".Copy");

            yield return new RibbonButtonDefinition(
                "Copy as XMI",
                CopyContentAsXmi,
                null,
                NavigationCategories.File + ".Copy");

            yield return new RibbonButtonDefinition(
                "Paste",
                PasteContent,
                null,
                NavigationCategories.File + ".Copy");

            if (DetailElement != null)
            {
                yield return new RibbonButtonDefinition(
                    "Show as Xmi",
                    ShowAsXmi,
                    null,
                    NavigationCategories.File);
            }

            void ViewConfig()
            {
                var dlg = new ItemXmlViewWindow
                {
                    SupportWriting = true,
                    Owner = Window.GetWindow(this)
                };
                dlg.UpdateContent(EffectiveForm);

                dlg.UpdateButtonPressed += (x, y) =>
                {
                    var temporaryExtent = InMemoryProvider.TemporaryExtent;
                    var factory = new MofFactory(temporaryExtent);
                    EffectiveForm = dlg.GetCurrentContentAsMof(factory);
                    UpdateContent();
                };

                dlg.ShowDialog();
            }

            void CopyContent()
            {
                var inMemory = InMemoryObject.CreateEmpty();
                StoreDialogContentIntoElement(inMemory);

                var copyContent = new CopyToClipboardCommand(inMemory);
                copyContent.Execute(CopyType.Default);
            }

            void CopyContentAsXmi()
            {
                var inMemory = InMemoryObject.CreateEmpty();
                StoreDialogContentIntoElement(inMemory);

                var copyContent = new CopyToClipboardCommand(inMemory);
                copyContent.Execute(CopyType.AsXmi);
            }

            void PasteContent()
            {
                var pasteContent = new PasteToClipboardCommand(DetailElement);
                pasteContent.Execute();
                UpdateContent();
            }

            void ShowAsXmi()
            {
                var itemXmlView = new ItemXmlViewWindow();
                itemXmlView.UpdateContent(DetailElement);
                itemXmlView.Show();
            }
        }

        /// <summary>
        ///     This event handler is thrown, when the user clicks on 'Save' and after the properties are
        ///     transferred from form display to element
        /// </summary>
        public event EventHandler ElementSaved;

        /// <summary>
        ///     Gets or sets the flag whether design shall be minimized
        /// </summary>
        /// <returns>true, if design shall be minimized</returns>
        public bool IsDesignMinimized()
        {
            return EffectiveForm?.getOrDefault<bool>(_FormAndFields._Form.minimizeDesign) == true;
        }

        /// <summary>
        ///     This event is called, when the view for the detail view is defined
        ///     While handling the event, the view itself may be changed
        /// </summary>
        public event EventHandler<ViewEventArgs> ViewDefined;

        private void DetailFormControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetContent(DetailElement, ViewDefinition?.Element);
            LoadingCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This event is called, after the Loaded event has been executed.
        /// This includes the creation and evaluation of forms and the filling out of content
        /// </summary>
        public event EventHandler LoadingCompleted;

        /// <summary>
        ///     Sets the content for a completely new object
        /// </summary>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IElement formDefinition)
        {
            SetContent(
                InMemoryObject.CreateEmpty(),
                formDefinition);
        }

        /// <summary>
        /// Sets the content by setting a form definition and the object itself.
        /// The form definition may be null, so a form will be automatically created. 
        /// </summary>
        /// <param name="element">Element to be shown. If null, then a new element will be created.</param>
        /// <param name="formDefinition">The form definition. If null, then the form will be automatically created </param>
        public void SetContent(IObject element, IElement formDefinition)
        {
            DetailElement = element ?? InMemoryObject.CreateEmpty();

            ViewDefinition = new ViewDefinition(
                null,
                formDefinition,
                formDefinition == null ? ViewDefinitionMode.Default : ViewDefinitionMode.Specific
            );

            AttachedElement = InMemoryObject.CreateEmpty();
            UpdateViewList();
            UpdateContent();
        }

        /// <summary>
        ///     Sets the form being used for the detail element.
        /// </summary>
        /// <param name="form">Form to be set</param>
        public void SetForm(IElement form)
        {
            ViewDefinition = new ViewDefinition(
                NamedElementMethods.GetFullName(form),
                form,
                ViewDefinitionMode.Specific);

            if (IsInitialized)
            {
                UpdateContent();
            }
        }

        /// <summary>
        ///     This method gets called to update the views
        /// </summary>
        private void UpdateViewList()
        {
            // Skip, if view selection shall be hidden
            if (_hideViewSelection == true) return;

            // If a specific viewdefinition is set, no view selection will shown.
            if (ViewDefinition.Mode == ViewDefinitionMode.Specific) return;

            // Update view
            var views = GetFormsForView()?.ToList();

            if (views != null)
            {
                if (ViewDefinition.Element != null && views.IndexOf(ViewDefinition.Element) == -1)
                {
                    views.Add(ViewDefinition.Element);
                }

                ViewList.Visibility = Visibility.Visible;
                var list = new List<ViewDefinition>
                {
                    new ViewDefinition("Default", null, ViewDefinitionMode.Default),
                    new ViewDefinition("All Properties", null, ViewDefinitionMode.AllProperties)
                };

                list.AddRange(views.Select(x => new ViewDefinition(NamedElementMethods.GetFullName(x), x)));
                ViewList.ItemsSource = list;

                switch (ViewDefinition.Mode)
                {
                    case ViewDefinitionMode.AllProperties:
                        ViewList.SelectedIndex = 1;
                        break;
                    case ViewDefinitionMode.Default:
                        ViewList.SelectedIndex = 0;
                        break;
                    default:
                        ViewList.SelectedIndex = 2 + views.IndexOf(ViewDefinition.Element);
                        break;
                }
            }
            else
            {
                ViewList.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        ///     Gets the enumeration of all views that may match to the shown items
        /// </summary>
        public IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope?.Resolve<IViewFinder>()
                .FindViews((DetailElement as IHasExtent)?.Extent as IUriExtent, DetailElement);
        }

        private void ViewList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(ViewList.SelectedItem is ViewDefinition newForm))
            {
                return;
            }

            ViewDefinition = newForm;
            UpdateContent();
        }

        /// <summary>
        ///     This method gets called, when a new item is added or an existing item was modified.
        ///     Derivation of the class which have automatic creation of columns may include additional columns
        /// </summary>
        private void RefreshViewDefinition()
        {
            UpdateActualViewDefinition();

            OnViewDefined();

            if (_hideViewSelection == null)
            {
                if (DotNetHelper.IsTrue(EffectiveForm.GetOrDefault(_FormAndFields._Form.fixView)))
                {
                    ViewList.Visibility = Visibility.Collapsed;
                    _hideViewSelection = true;
                }
                else
                {
                    _hideViewSelection = false;
                }
            }
        }

        private void UpdateActualViewDefinition()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            if (ViewDefinition.Mode == ViewDefinitionMode.Default)
            {
                EffectiveForm = viewFinder.FindDetailView(DetailElement);
            }

            switch (ViewDefinition.Mode)
            {
                case ViewDefinitionMode.AllProperties:
                case ViewDefinitionMode.Default when EffectiveForm == null:
                    EffectiveForm = viewFinder.CreateView(DetailElement);
                    break;
                case ViewDefinitionMode.Specific:
                    EffectiveForm = ViewDefinition.Element;
                    break;
            }
        }

        /// <summary>
        ///     Updates the content
        /// </summary>
        private void UpdateContent()
        {
            RefreshViewDefinition();

            DataGrid.Children.Clear();
            AttachedItemFields.Clear();
            ItemFields.Clear();

            if (!(EffectiveForm?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            var isMinimized = EffectiveForm.getOrDefault<bool>(_FormAndFields._Form.minimizeDesign);
            if (isMinimized)
            {
                ViewList.Visibility = Visibility.Collapsed;
            }

            _fieldCount = 0;

            CreateRows(fields);

            // Adds metadata
            if (DetailElement != null)
            {
                var mofElement = (MofElement) DetailElement;
                var uriExtent = DetailElement.GetUriExtentOf();

                var hideMetaClass = EffectiveForm.getOrDefault<bool>(_FormAndFields._Form.hideMetaClass);

                if (!hideMetaClass && !isMinimized)
                {
                    CreateSeparator();

                    var uriExtentText = uriExtent?.contextURI() ?? string.Empty;
                    var fullName = NamedElementMethods.GetFullName(DetailElement);
                    CreateRowForField("Extent:", uriExtentText, true);
                    CreateRowForField("Full Name:", fullName, true);
                    CreateRowForField("Url w/ ID:", mofElement.GetUri(), true);
                    CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

                    // Sets the metaclass
                    if (DotNetHelper.IsFalseOrNotSet(EffectiveForm, _FormAndFields._Form.hideMetaClass))
                    {
                        var metaClass = (DetailElement as IElement)?.getMetaClass();
                        CreateRowForField(
                            "Meta Class:",
                            metaClass == null ? string.Empty : NamedElementMethods.GetFullName(metaClass),
                            true);
                    }
                }
            }
        }

        /// <summary>
        ///     Creates the rows
        /// </summary>
        /// <param name="fields">Fields to be created</param>
        private void CreateRows(IReflectiveCollection fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            var anyFocused = false;
            foreach (var field in fields.Cast<IElement>())
            {
                var flags = new FieldParameter();

                var fieldType = field.getOrDefault<string>(_FormAndFields._FieldData.fieldType);
                if (fieldType == MetaClassElementFieldData.FieldType)
                {
                    continue;
                }
                
                var (detailElement, contentBlock) =
                    FieldFactory.GetUIElementFor(DetailElement, field, this, flags);

                if (!flags.IsSpanned)
                {

                    var title = field.getOrDefault<string>(_FormAndFields._FieldData.title);
                    var isReadOnly = field.getOrDefault<bool>(_FormAndFields._FieldData.isReadOnly);

                    // Sets the title block
                    var titleBlock = new TextBlock
                    {
                        Text = string.IsNullOrEmpty(title) ? "" : $"{title}: ",
                        IsEnabled = !isReadOnly
                    };

                    CreateRowForField(titleBlock, contentBlock);
                }
                else
                {
                    CreateSpannedRow(contentBlock);
                }
            
                // Checks whether the control element shall be stored in
                // the detail element iself or within the attached fields
                if (field.getOrNull<bool>(_FormAndFields._FieldData.isAttached) == true)
                {
                    AttachedItemFields.Add(detailElement);
                }
                else
                {
                    ItemFields.Add(detailElement);
                }

                // Check, if element shall be focused
                if (!anyFocused && flags.CanBeFocused)
                {
                    contentBlock.Focus();
                    anyFocused = true;
                }
            }

            AddRowsForInteractionHandlers();
        }

        /// <summary>
        /// Adds the buttons for the interactionhandlers. 
        /// </summary>
        private void AddRowsForInteractionHandlers()
        {
            var buttons = new List<Button>();

            // Creates additional rows for buttons with additional actions
            var interactionHandlers = App.Scope.Resolve<IEnumerable<IElementInteractionsHandler>>();
            foreach (var handler in interactionHandlers
                .SelectMany(x => x.GetInteractions(DetailElement)))
            {
                var button = new Button {Content = handler.Name};
                button.Click += (x, y) => handler.Execute(DetailElement, null);
                buttons.Add(button);
            }

            if (buttons.Count > 0)
            {
                var panel = new StackPanel();
                foreach (var button in buttons)
                {
                    panel.Children.Add(button);
                }

                CreateRowForField(new TextBlock {Text = "Actions:"}, panel);
            }
        }

        /// <summary>
        ///     Creates a row by having a text
        /// </summary>
        /// <param name="keyText">Text to be added</param>
        /// <param name="valueText">Value to be added</param>
        /// <param name="selectable">True, if the user can copy the content to the clipboard.</param>
        private void CreateRowForField(string keyText, string valueText, bool selectable = false)
        {
            var valueTextBlock = new TextBlock {Text = valueText};

            if (selectable)
            {
                valueTextBlock.ContextMenu =
                    new ContextMenu();

                var copyToClipboardAdd = new MenuItem {Header = "Copy to Clipboard"};
                valueTextBlock.ContextMenu.Items.Add(copyToClipboardAdd);
                copyToClipboardAdd.Click += (x, y) => Clipboard.SetText(valueText);
            }

            CreateRowForField(
                new TextBlock {Text = keyText},
                valueTextBlock);
        }

        /// <summary>
        ///     Creates a new row in the detail view
        /// </summary>
        /// <param name="propertyKey">UIElement for the left column</param>
        /// <param name="propertyValue">UIElement for the right column</param>
        private void CreateRowForField(UIElement propertyKey, UIElement propertyValue)
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());

            Grid.SetColumn(propertyKey, 0);
            Grid.SetRow(propertyKey, _fieldCount);

            Grid.SetColumn(propertyValue, 1);
            Grid.SetRow(propertyValue, _fieldCount);

            DataGrid.Children.Add(propertyKey);
            DataGrid.Children.Add(propertyValue);

            _fieldCount++;
        }

        /// <summary>
        ///     Creates a new row in which the element is spanned through the complete
        ///     columns.
        /// </summary>
        /// <param name="element">Element to be included</param>
        private void CreateSpannedRow(UIElement element)
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());

            Grid.SetColumn(element, 0);
            Grid.SetRow(element, _fieldCount);
            Grid.SetColumnSpan(element, 3);

            DataGrid.Children.Add(element);
            _fieldCount++;
        }

        private void CreateSeparator()
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());
            var line = new Canvas
            {
                Background = Brushes.DimGray,
                Height = 1.0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 10, 0, 10)
            };

            Grid.SetRow(line, _fieldCount);
            Grid.SetColumnSpan(line, 3);

            DataGrid.Children.Add(line);

            _fieldCount++;
        }

        /// <summary>
        ///     Adds the default cancel and save buttons
        /// </summary>
        public void AddDefaultButtons(string saveText = "Save")
        {
            if (AllowNewProperties)
            {
                AddGenericButton("New Property", () =>
                {
                    var fieldKey = new TextBox();
                    var fieldValue = new TextboxField();
                    var flags = new FieldParameter();

                    var fieldData = MofFactory.CreateElementFor<_FormAndFields>(
                        EffectiveForm,
                        x => x.__TextFieldData);

                    var fieldUIElement = fieldValue.CreateElement(
                        DetailElement,
                        fieldData,
                        this, 
                        flags);

                    ItemFields.Add(new NewPropertyField(
                        fieldKey,
                        fieldUIElement));

                    CreateRowForField(fieldKey, fieldUIElement);
                    fieldKey.Focus();
                });
            }

            AddGenericButton("Cancel", () =>
            {
                var window = Window.GetWindow(this);
                window?.Close();
            }).IsCancel = true;

            AddGenericButton(saveText, () =>
            {
                try
                {
                    StoreDialogContentIntoElement(DetailElement);

                    OnElementSaved();
                    var window = Window.GetWindow(this);
                    window?.Close();

                    if (window is DetailFormWindow detailFormWindow)
                    {
                        detailFormWindow.OnSaved(DetailElement, AttachedElement);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }).IsDefault = true;
        }

        /// <summary>
        /// This helper class is used to allow the setting of new properties
        /// by the user. It takes two text boxes and sets
        /// the new property as defined by the user within the given object
        /// </summary>
        private class NewPropertyField : IDetailField
        {
            private readonly TextBox _fieldKey;
            private readonly UIElement _fieldUiElement;

            public NewPropertyField(TextBox fieldKey, UIElement fieldUiElement)
            {
                _fieldKey = fieldKey;
                _fieldUiElement = fieldUiElement;
            }

            public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags)
            {
                throw new NotImplementedException();
            }

            public void CallSetAction(IObject element)
            {
                var propertyKey = _fieldKey.Text;
                var propertyValue = (_fieldUiElement as TextBox)?.Text;

                if (string.IsNullOrEmpty(propertyKey) || propertyValue == null)
                {
                    return;
                }

                element.set(propertyKey, propertyValue);
            }
        }

        /// <summary>
        ///     Takes the input that the user has currently into the dialog and stores these changes into the given element.
        /// </summary>
        /// <param name="element">Element in which the content of the element shall be stored</param>
        private void StoreDialogContentIntoElement(IObject element)
        {
            if (!Equals(element, DetailElement))
            {
                // Copy all data from DetailElement to element to also have the non-shown properties in the mirror object
                ObjectCopier.CopyPropertiesStatic(DetailElement, element);
            }

            foreach (var field in ItemFields)
            {
                field.CallSetAction(element);
            }

            foreach (var field in AttachedItemFields)
            {
                field.CallSetAction(AttachedElement);
            }
        }

        /// <summary>
        ///     Adds a button to the view
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="onPressed">Called if the user clicks on the button</param>
        /// <returns>The created button</returns>
        public ViewButton AddGenericButton(string name, Action onPressed)
        {
            var button = new ViewButton {Content = name};

            button.Pressed += (x, y) => { onPressed(); };
            ButtonBar.Children.Add(button);
            return button;
        }

        protected void OnElementSaved()
        {
            ElementSaved?.Invoke(this, EventArgs.Empty);
        }

        public void OnViewDefined()
        {
            OnViewDefined(new ViewEventArgs(EffectiveForm));
        }

        protected virtual void OnViewDefined(ViewEventArgs e)
        {
            ViewDefined?.Invoke(this, e);
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            new CopyToClipboardCommand(this).Execute(CopyType.Default);
        }

        public class ViewEventArgs : EventArgs
        {
            public ViewEventArgs(IElement view)
            {
                View = view;
            }

            public IElement View { get; }
        }
    }
}