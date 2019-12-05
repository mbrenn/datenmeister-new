using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.UserInteractions;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Commands;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Forms.Detail.Fields;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    ///     Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl, INavigationGuest, IHasSelectedItems, IHasTitle, IItemNavigationGuest
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DetailFormControl));

        /// <summary>
        /// Stores  the number of fields
        /// </summary>
        private int _fieldCount;

        /// <summary>
        /// Stores the title of the form control. If not overridden, a default
        /// title will be created
        /// </summary>
        private string _internalTitle;

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
        /// Gets or sets the container for the Detail Element. It will be used to
        /// delete the item, if required.
        /// </summary>
        public IReflectiveCollection DetailElementContainer { get; set; }

        /// <summary>
        ///     Defines the form definition being used in the detail for
        /// </summary>
        public IObject EffectiveForm { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether new properties may be added by the user to the element
        /// </summary>
        public bool AllowNewProperties { get; set; }

        /// <summary>
        /// Gets the attached element which is allocated in the navigation host
        /// </summary>
        public IElement AttachedElement {get; set; }

        /// <summary>
        ///     Gets the default size
        /// </summary>
        public Size DefaultSize => new Size(
            EffectiveForm?.getOrDefault<double>(_FormAndFields._DetailForm.defaultWidth) ?? 0.0,
            EffectiveForm?.getOrDefault<double>(_FormAndFields._DetailForm.defaultHeight) ?? 0.0
        );

        /// <summary>
        /// Gets the list of the item fields being used to store the information into the item
        /// </summary>
        public List<IDetailField> ItemFields { get; } = new List<IDetailField>();

        /// <summary>
        /// Gets the list of field information being allocated to the form but not to the detail form
        /// </summary>
        public List<IDetailField> AttachedItemFields { get; } = new List<IDetailField>();

        public IObject GetSelectedItem() => DetailElement;

        public IEnumerable<IObject> GetSelectedItems()
        {
            yield return DetailElement;
        }

        /// <summary>
        ///     Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets the title for the control
        /// </summary>
        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(_internalTitle))
                {
                    return _internalTitle;
                }

                return DetailElement == null
                    ? "New item"
                    : $"Edit Item: {NamedElementMethods.GetFullName(DetailElement)}";
            }

            set => _internalTitle = value;
        }

        /// <summary>
        ///     Prepares the navigation of the host. The function is called by the navigation
        ///     host.
        /// </summary>
        public IEnumerable<ViewExtension> GetViewExtensions()
        {
            yield return new ItemMenuButtonDefinition(
                "Copy",
                CopyContent,
                null,
                NavigationCategories.DatenMeister);

            yield return new ItemMenuButtonDefinition(
                "Copy as XMI",
                CopyContentAsXmi,
                null,
                NavigationCategories.DatenMeister);

            yield return new ItemMenuButtonDefinition(
                "Paste",
                PasteContent,
                null,
                NavigationCategories.DatenMeister);

            yield return new ApplicationMenuButtonDefinition(
                "Show Form Definition",
                ViewConfig,
                null,
                NavigationCategories.Form);

            yield return new ApplicationMenuButtonDefinition(
                "Save Form Definition",
                CopyForm,
                null,
                NavigationCategories.Form);


            if (DetailElementContainer != null)
            {
                yield return new ItemButtonDefinition(
                    "Delete",
                    (x) =>
                    {
                        if (
                            MessageBox.Show(
                                "Do you want to delete the item?",
                                "Delete item",
                                MessageBoxButton.YesNo) ==
                            MessageBoxResult.Yes)
                        {
                            DetailElementContainer.remove(DetailElement);
                            (NavigationHost as Window)?.Close();
                        }
                    });
            }

            if (DetailElement != null)
            {
                yield return new ItemMenuButtonDefinition(
                    "Show as Xmi",
                    ShowAsXmi,
                    null,
                    NavigationCategories.DatenMeister);
            }

            // Local methods for the buttons
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
                    UpdateView();
                };

                dlg.ShowDialog();
            }

            void CopyContent(IObject element)
            {
                var inMemory = InMemoryObject.CreateEmpty(element.GetExtentOf());
                StoreDialogContentIntoElement(inMemory);

                CopyToClipboardCommand.Execute(inMemory, CopyType.Default);
            }

            void CopyContentAsXmi(IObject element)
            {
                var inMemory = InMemoryObject.CreateEmpty(element.GetExtentOf());
                StoreDialogContentIntoElement(inMemory);

                CopyToClipboardCommand.Execute(inMemory, CopyType.AsXmi);
            }

            void PasteContent(IObject element)
            {
                var pasteContent = new PasteToClipboardCommand(element);
                pasteContent.Execute();
                UpdateView();
            }

            void ShowAsXmi(IObject element)
            {
                var itemXmlView = new ItemXmlViewWindow();
                itemXmlView.UpdateContent(element);
                itemXmlView.Show();
            }

            void CopyForm()
            {
                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                var target = viewLogic.GetUserViewExtent();
                var copier = new ObjectCopier(new MofFactory(target));

                var copiedForm = copier.Copy(EffectiveForm);
                target.elements().add(copiedForm);

                NavigatorForItems.NavigateToElementDetailView(NavigationHost, copiedForm);
            }
        }

        /// <summary>
        ///     This event handler is thrown, when the user clicks on 'Save' and after the properties are
        ///     transferred from form display to element
        /// </summary>
        public event EventHandler ElementSaved;

        /// <summary>
        ///     This event is called, when the view for the detail view is defined
        ///     While handling the event, the view itself may be changed
        /// </summary>
        public event EventHandler<ViewEventArgs> ViewDefined;

        private void DetailFormControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This event is called, after the Loaded event has been executed.
        /// This includes the creation and evaluation of forms and the filling out of content
        /// </summary>
        public event EventHandler LoadingCompleted;

        /// <summary>
        ///     This method gets called, when a new item is added or an existing item was modified.
        ///     Derivation of the class which have automatic creation of columns may include additional columns
        /// </summary>
        private void RefreshViewDefinition()
        {
            OnViewDefined();
        }

        public void SetContent(IObject value, IObject detailForm, IReflectiveCollection collection = null)
        {
            DetailElement = value;
            EffectiveForm = detailForm;
            DetailElementContainer = collection;
            UpdateView();
        }

        public void SetForm(IObject detailForm)
        {
            EffectiveForm = detailForm;
            UpdateView();
        }

        /// <summary>
        ///     Updates the content
        /// </summary>
        public void UpdateView()
        {
            RefreshViewDefinition();

            // Checks, if the form overwrites the allow new properties information. If yes, store it
            var t = EffectiveForm?.getOrNull<bool>(_FormAndFields._DetailForm.allowNewProperties);
            AllowNewProperties = t ?? AllowNewProperties;

            DataGrid.Children.Clear();
            AttachedItemFields.Clear();
            ItemFields.Clear();

            var fields = EffectiveForm?.getOrDefault<IReflectiveCollection>(_FormAndFields._Form.field);
            if (fields == null)
            {
                return;
            }

            _fieldCount = 0;

            // Here, create the rows themselves
            CreateRows(fields);

            // Adds metadata
            if (DetailElement != null)
            {
                var mofElement = DetailElement as MofElement; // Used to get the uri including id
                var uriExtent = DetailElement.GetUriExtentOf();

                var hideMetaClass = EffectiveForm.getOrDefault<bool>(_FormAndFields._Form.hideMetaInformation);

                if (!hideMetaClass)
                {
                    CreateSeparator();

                    var uriExtentText = uriExtent?.contextURI() ?? string.Empty;
                    var fullName = NamedElementMethods.GetFullName(DetailElement);
                    CreateRowForField("Extent:", uriExtentText, true);
                    CreateRowForField("Full Name:", fullName, true);
                    CreateRowForField("Url w/ ID:", mofElement?.GetUri() ?? "Not known", true);
                    CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

                    var metaClass = (DetailElement as IElement)?.getMetaClass();
                    CreateRowForField(
                        "Meta Class:",
                        metaClass == null ? string.Empty : NamedElementMethods.GetFullName(metaClass),
                        true);
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

                var (detailElement, contentBlock) =
                    FieldFactory.GetUIElementFor(DetailElement, field, this, flags);

                if (contentBlock != null)
                {
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
                }

                // Checks whether the control element shall be stored in
                // the detail element itself or within the attached fields
                if (field.getOrNull<bool>(_FormAndFields._FieldData.isAttached) == true)
                {
                    AttachedItemFields.Add(detailElement);
                }
                else
                {
                    ItemFields.Add(detailElement);
                }

                // Check, if element shall be focused
                if (!anyFocused && flags.CanBeFocused && contentBlock != null)
                {
                    // For what ever, we have to set the focus via the invoking and not directly
                    Dispatcher?.BeginInvoke((Action) (() =>
                    {
                        if (!contentBlock.Focus())
                        {
                            Logger.Debug("No keyboard focus set");
                        }
                    }));

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
            var interactionHandlers = GiveMe.Scope.Resolve<IEnumerable<IElementInteractionsHandler>>();
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
                    panel.Children.Add(button);

                CreateRowForField(new TextBlock {Text = "Actions:"}, panel);
            }
        }

        /// <summary>
        ///     Creates a row by having a text
        /// </summary>
        /// <param name="keyText">Text to be added</param>
        /// <param name="valueText">Value to be added</param>
        /// <param name="selectable">True, if the user can copy the content to the clipboard.</param>
        public void CreateRowForField(string keyText, string valueText, bool selectable = false)
        {
            var valueTextBlock = new TextBlock {Text = valueText};

            if (selectable)
            {
                valueTextBlock.ContextMenu = new ContextMenu();

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
        public void CreateRowForField(UIElement propertyKey, UIElement propertyValue)
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
        public void CreateSpannedRow(UIElement element)
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());

            Grid.SetColumn(element, 0);
            Grid.SetRow(element, _fieldCount);
            Grid.SetColumnSpan(element, 3);

            DataGrid.Children.Add(element);
            _fieldCount++;
        }

        /// <summary>
        /// Creates a separation line
        /// </summary>
        public void CreateSeparator()
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
        /// Clears the existing generic buttons
        /// </summary>
        public void ClearGenericButtons()
        {
            ButtonBar.Children.Clear();
        }

        /// <summary>
        ///     Adds the default cancel and save buttons
        /// </summary>
        /// <param name="saveText">Text which is shown on the save button. Default value is "Save"</param>
        public void AddDefaultButtons(string saveText = null)
        {
            saveText ??= EffectiveForm.getOrDefault<string>(_FormAndFields._DetailForm.buttonApplyText);
            saveText ??= "Save";

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

                    var fieldUiElement = fieldValue.CreateElement(
                        DetailElement,
                        fieldData,
                        this,
                        flags);

                    ItemFields.Add(new NewPropertyField(
                        fieldKey,
                        fieldUiElement));

                    CreateRowForField(fieldKey, fieldUiElement);
                    fieldKey.Focus();
                });
            }

            AddGenericButton(saveText, () =>
            {
                try
                {
                    StoreDialogContentIntoElement(DetailElement);

                    OnElementSaved();
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

            public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, FieldParameter fieldFlags) =>
                throw new NotImplementedException();

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

            // Calls the attached elements, if this method is not invoked 'externally'
            if (element.@equals(DetailElement))
            {
                foreach (var field in AttachedItemFields)
                {
                    field.CallSetAction(AttachedElement);
                }
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

            button.Pressed += (x, y) => onPressed();
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
            CopyToClipboardCommand.Execute(this, CopyType.Default);
        }

        public class ViewEventArgs : EventArgs
        {
            public ViewEventArgs(IObject view)
            {
                View = view;
            }

            public IObject View { get; }
        }

        public void EvaluateViewExtensions(IEnumerable<ViewExtension> extensions)
        {
            ClearGenericButtons();
            AddDefaultButtons();
            foreach (var extension in extensions)
            {
                if (extension is ItemButtonDefinition itemButtonDefinition)
                {
                    AddGenericButton(itemButtonDefinition.Name,
                        () => itemButtonDefinition.OnPressed(DetailElement));
                }
            }
        }

        public IObject Item => DetailElement;
    }
}