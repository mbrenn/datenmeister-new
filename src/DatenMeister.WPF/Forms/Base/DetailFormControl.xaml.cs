#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.Forms;
using DatenMeister.Modules.Validators;
using DatenMeister.WPF.Commands;
using DatenMeister.WPF.Forms.Fields;
using DatenMeister.WPF.Modules.UserInteractions;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Modules.ViewExtensions.Information;
using DatenMeister.WPF.Navigation;
using DatenMeister.WPF.Windows;
using Button = System.Windows.Controls.Button;
using Clipboard = System.Windows.Clipboard;
using ContextMenu = System.Windows.Controls.ContextMenu;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

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
        private string? _internalTitle;

        private IObject? _effectiveForm;
        
        /// <summary>
        /// Gets the form parameter for the detail form
        /// </summary>
        public FormParameter? FormParameter { get; private set; }

        /// <summary>
        /// Defines the event that will be called whenever the property value has changed
        /// </summary>
        public event EventHandler<PropertyValueChangedEventArgs>? PropertyValueChanged;

        public DetailFormControl()
        {
            InitializeComponent();
            Loaded += DetailFormControl_Loaded;
        }

        /// <summary>
        ///     Gets the detailed element, whose content is shown in the dialog
        /// </summary>
        public IObject DetailElement
        {
            get => _detailElement ?? throw new InvalidOperationException("DetailElement");
            set => _detailElement = value;
        }

        /// <summary>
        /// Gets or sets the container for the Detail Element. It will be used to
        /// delete the item, if required.
        /// </summary>
        public IReflectiveCollection? DetailElementContainer { get; set; }

        /// <summary>
        ///     Defines the form definition being used in the detail for
        /// </summary>
        public IObject? EffectiveForm
        {
            get => _effectiveForm;
            set
            {
                _effectiveForm = value;
#if DEBUG
                if (value != null && !FormMethods.ValidateForm(value)) 
                    throw new InvalidOperationException("The form did not pass validation");
#endif
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether new properties may be added by the user to the element
        /// </summary>
        public bool AllowNewProperties { get; set; }

        /// <summary>
        /// Gets the attached element which is allocated in the navigation host
        /// </summary>
        public IElement? AttachedElement {get; set; }

        /// <summary>
        ///     Gets the default size
        /// </summary>
        public Size DefaultSize => new Size(
            EffectiveForm?.getOrDefault<double>(_DatenMeister._Forms._DetailForm.defaultWidth) ?? 0.0,
            EffectiveForm?.getOrDefault<double>(_DatenMeister._Forms._DetailForm.defaultHeight) ?? 0.0
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
        
        /// <summary>
        /// Stores the list of validators
        /// </summary>
        public readonly IList<IElementValidator> ElementValidators = new List<IElementValidator>();

        private IObject? _detailElement;
        
        private INavigationHost? _navigationHost;

        public IEnumerable<IObject> GetSelectedItems()
        {
            if (DetailElement != null)
            {
                yield return DetailElement;
            }
        }

        /// <summary>
        ///     Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost
        {
            get => _navigationHost ?? throw new InvalidOperationException("NavigationHost == null");
            set => _navigationHost = value;
        }

        /// <summary>
        /// Gets the title for the control
        /// </summary>
        public string Title
        {
            get
            {
                if (!string.IsNullOrEmpty(_internalTitle))
                {
                    return _internalTitle ?? string.Empty;
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
                "Export as XMI",
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
                NavigationCategories.Form + ".Current");

            yield return new ApplicationMenuButtonDefinition(
                "Save Form Definition",
                CopyForm,
                null,
                NavigationCategories.Form + ".Current");


            if (DetailElementContainer != null && DetailElement != null)
            {
                yield return new ItemButtonDefinition(
                    "Delete",
                    (x) =>
                    {
                        var name = NamedElementMethods.GetName(x);
                        if (
                            MessageBox.Show(
                                $"Do you want to delete the item '{name}'?",
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
  
            // 2) Get the view extensions by the plugins
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var viewExtensionInfo = new ViewExtensionInfoItem(NavigationHost, this)
            {
                Item = DetailElement
            };

            foreach (var plugin in viewExtensionPlugins)
            {
                foreach (var extension in plugin.GetViewExtensions(viewExtensionInfo))
                {
                    yield return extension;
                }
            }

            ///////////////////

            // Local methods for the buttons
            void ViewConfig()
            {
                if (EffectiveForm == null)
                {
                    MessageBox.Show("EffectiveForm is not set");
                    return;
                }
                
                var dlg = new ItemXmlViewWindow
                {
                    SupportWriting = true,
                    Owner = Window.GetWindow(this)
                };

                dlg.UpdateContent(EffectiveForm);

                dlg.UpdateButtonPressed += (x, y) =>
                {
                    if (EffectiveForm == null)
                    {
                        MessageBox.Show("No detail item is linked to the current view, so updating is not possible.");
                        return;
                    }
                    
                    var factory = new MofFactory(EffectiveForm);
                    EffectiveForm = dlg.GetCurrentContentAsMof(factory);
                    UpdateForm();
                    
                    dlg.Close();
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
                UpdateForm();
            }

            void ShowAsXmi(IObject element)
            {
                var itemXmlView = new ItemXmlViewWindow();
                itemXmlView.UpdateContent(element);
                itemXmlView.Show();
            }

            void CopyForm()
            {
                if (EffectiveForm == null)
                {
                    MessageBox.Show("EffectiveForm is not set");
                    return;
                }
                
                var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                var target = viewLogic.GetUserFormExtent();
                var copier = new ObjectCopier(new MofFactory(target));

                var copiedForm = copier.Copy(EffectiveForm);
                target.elements().add(copiedForm);

                _ = NavigatorForItems.NavigateToElementDetailView(NavigationHost, copiedForm);
            }
        }

        /// <summary>
        ///     This event handler is thrown, when the user clicks on 'Save' and after the properties are
        ///     transferred from form display to element
        /// </summary>
        public event EventHandler? ElementSaved;

        /// <summary>
        ///     This event is called, when the view for the detail view is defined
        ///     While handling the event, the view itself may be changed
        /// </summary>
        public event EventHandler<ViewEventArgs>? ViewDefined;

        private void DetailFormControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        ///     This method gets called, when a new item is added or an existing item was modified.
        ///     Derivation of the class which have automatic creation of columns may include additional columns
        /// </summary>
        private void RefreshViewDefinition()
        {
            OnViewDefined();
        }

        public void SetContent(IObject value, IObject detailForm, IReflectiveCollection? collection = null, FormParameter? formParameter = null)
        {
            DetailElement = value;
            EffectiveForm = detailForm;
            DetailElementContainer = collection;
            FormParameter = formParameter;
            UpdateForm();
        }

        public void SetForm(IObject detailForm)
        {
            EffectiveForm = detailForm;
            UpdateForm();
        }

        /// <summary>
        ///     Updates the content
        /// </summary>
        public void UpdateForm()
        {
            var stopWatch = Stopwatch.StartNew();
            
            RefreshViewDefinition();

            // Checks, if the form overwrites the allow new properties information. If yes, store it
            var t = EffectiveForm?.getOrNull<bool>(_DatenMeister._Forms._DetailForm.allowNewProperties);
            AllowNewProperties = t ?? AllowNewProperties;

            DataGrid.Children.Clear();
            AttachedItemFields.Clear();
            ItemFields.Clear();

            var fields = EffectiveForm?.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field);
            if (fields == null)
            {
                return;
            }

            _fieldCount = 0;

            // Here, create the rows themselves
            CreateRows(fields);
            
            stopWatch.Stop();
            Logger.Info("UpdateView Duration", stopWatch.ElapsedMilliseconds, "ms");
        }

        /// <summary>
        ///     Creates the rows
        /// </summary>
        /// <param name="fields">Fields to be created</param>
        private void CreateRows(IReflectiveCollection fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (DetailElement == null) throw new InvalidOperationException("DetailElement == null");

            var isFormReadOnly = EffectiveForm?.getOrDefault<bool>(_DatenMeister._Forms._Form.isReadOnly) == true
                                 || FormParameter?.IsReadOnly == true;

            DataGrid.Children.Clear();
            var anyFocused = false;
            foreach (var field in fields.Cast<IElement>())
            {
                var flags = new FieldParameter {IsReadOnly = isFormReadOnly};
                var isAttached = field.getOrNull<bool>(_DatenMeister._Forms._FieldData.isAttached) == true;

                var usedElement = isAttached ? AttachedElement : DetailElement;
                if (usedElement == null) continue;
                
                var (detailElement, contentBlock) =
                    FieldFactory.GetUIElementFor(
                        usedElement,
                        field, 
                        this,
                        flags);

                if (detailElement is IPropertyValueChangeable propertyValueChangeable)
                {
                    propertyValueChangeable.PropertyValueChanged += (x, y) =>
                    {
                        var ev = PropertyValueChanged;
                        ev?.Invoke(x, y);
                    };
                }
                
                if (contentBlock != null)
                {
                    if (!flags.IsSpanned)
                    {
                        var title = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title);
                        var isReadOnly = 
                            field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isReadOnly) || isFormReadOnly;

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
                if (field.getOrNull<bool>(_DatenMeister._Forms._FieldData.isAttached) == true)
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
            if (DetailElement == null)
            {
                // No element, no interaction
                return;
            }

            // Creates additional rows for buttons with additional actions
            var scope = GiveMe.Scope;
            var interactionHandlers = 
                scope.ScopeStorage.Get<UserInteractionState>().ElementInteractionHandler;
            foreach (var handler in interactionHandlers
                .SelectMany(x => x.GetInteractions(DetailElement)))
            {
                var button = new Button {Content = handler.Name};
                button.Click += (x, y) 
                    => handler.Execute(this, DetailElement, null);
                buttons.Add(button);
            }

            var viewExtensions = GetViewExtensions();
            if (viewExtensions != null)
            {
                foreach (var viewExtension in viewExtensions.OfType<RowItemButtonDefinition>())
                {
                    var button = new Button {Content = viewExtension.Name};
                    button.Click += (x, y)
                        => viewExtension.OnPressed(this, DetailElement);
                    buttons.Add(button);
                }
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
        public TextBlock CreateRowForField(string keyText, string valueText, bool selectable = false)
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

            return valueTextBlock;
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
        public void AddDefaultButtons(string? saveText = null)
        {
            if (DetailElement == null)
                throw new InvalidOperationException("DetailElement == null");
            if (EffectiveForm == null)
                throw new InvalidOperationException("EffectiveForm == null");

            saveText ??= EffectiveForm.getOrDefault<string>(_DatenMeister._Forms._DetailForm.buttonApplyText);
            saveText ??= "Save";

            if (AllowNewProperties)
            {
                AddGenericButton("New Property", () =>
                {
                    if (EffectiveForm == null)
                        throw new InvalidOperationException("EffectiveForm == null");
                    
                    var fieldKey = new TextBox();
                    var fieldValue = new TextboxField();
                    var flags = new FieldParameter();

                    var fieldData = _DatenMeister.TheOne.Forms.__TextFieldData;

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
                    var validityOfContent = CheckValidityOfContent(DetailElement);

                    if (validityOfContent)
                    {
                        StoreDialogContentIntoElement(DetailElement);
                        OnElementSaved();
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }).IsDefault = true;
        }

        /// <summary>
        /// Checks, if the current content of the detail form is valid.
        /// If the content is not valid, then a message can be returned to the user and the closing of the window
        /// will not be performed
        /// </summary>
        /// <param name="detailElement">Element to be set</param>
        /// <returns>true, if the validation was successful</returns>
        private bool CheckValidityOfContent(IObject detailElement)
        {
            // Checks, if the provider is capable 
            var capabilities = (detailElement.GetExtentOf() as MofExtent)?.Provider.GetCapabilities();
            if (capabilities != null && !capabilities.CanCreateElements) return true;
            
            var inMemory = MofFactory.Create(
                detailElement.GetExtentOf() ?? InMemoryProvider.TemporaryExtent,
                (detailElement as IElement)?.metaclass);
            
            StoreDialogContentIntoElement(inMemory);
            var success = true;
            var messages = new StringBuilder();
            var nrRecommendations = 0;
            var nrErrors = 0;
            var recommendations = new StringBuilder();
            var errors = new StringBuilder();

            foreach (var validator in ElementValidators)
            {
                var result = validator.ValidateElement(inMemory);
                
                while (result != null)
                {
                    switch (result.State)
                    {
                        case ValidatorState.Recommendation:
                            recommendations.Append($"- {result.PropertyName}: {result.Message}\r\n");
                            nrRecommendations++;
                            break;
                        case ValidatorState.Error:
                            errors.Append($"- {result.PropertyName}: {result.Message}\r\n");
                            nrErrors++;
                            success = false;
                            break;
                    }

                    result = result.Next;
                }
            }

            if (nrErrors > 0)
            {
                messages.Append($"The following errors were given:\r\n{errors}\r\n");
            }
            
            if (nrRecommendations > 0)
            {
                messages.Append($"The following recommendations were given:\r\n{recommendations}\r\n");
            }
            
            if (messages.Length > 0)
            {
                if (nrErrors == 0)
                {
                    if (MessageBox.Show(
                            $"{messages}\r\nContinue saving and closing?",
                            "Field validation with recommendation", MessageBoxButton.YesNo, MessageBoxImage.Information)
                        == MessageBoxResult.No)
                    {
                        success = false;
                    }
                }
                else
                {
                    MessageBox.Show(
                        $"{messages}\r\nPlease fix the given errors before submitting the form",
                        "Field validation failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            
            // Default: Everything is valid
            return success;
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
        public void StoreDialogContentIntoElement(IObject element)
        {
            if (DetailElement != null && !Equals(element, DetailElement))
            {
                // Copy all data from DetailElement to element to also have the non-shown properties in the mirror object
                ObjectCopier.CopyPropertiesStatic(DetailElement, element);
            }

            foreach (var field in ItemFields)
            {
                field.CallSetAction(element);
            }

            // Calls the attached elements, if this method is not invoked 'externally'
            if (element.@equals(DetailElement) && AttachedElement != null)
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
            if (EffectiveForm == null)
                throw new InvalidOperationException("EffectiveForm == null");
            
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

        public void EvaluateViewExtensions(ICollection<ViewExtension> extensions)
        {
            ClearGenericButtons();
            AddDefaultButtons();
            foreach (var extension in extensions)
            {
                if (extension is ItemButtonDefinition itemButtonDefinition)
                {
                    AddGenericButton(itemButtonDefinition.Name,
                        () =>
                        {
                            if (DetailElement != null)
                            {
                                itemButtonDefinition.OnPressed(DetailElement);
                            }
                        });
                }
            }
        }

        public IObject Item => DetailElement;

        /// <summary>
        /// Injects a new property value by parsing through all fields and
        /// inject property Vaue 
        /// </summary>
        /// <param name="property">Property to be injected</param>
        /// <param name="value">Value to be injected</param>
        public void InjectPropertyValue(string property, object value)
        {
            foreach (var field in ItemFields.OfType<IInjectPropertyValue>())
            {
                field.InjectValue(property, value);
            }
        }
    }
}