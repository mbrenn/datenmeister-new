using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Detail.Fields;
using DatenMeisterWPF.Navigation;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl, INavigationGuest
    {
        /// <summary>
        /// Defines the form definition being used in the detail for
        /// </summary>
        private IElement ActualFormDefinition { get; set; }

        /// <summary>
        /// Gets or sets the view definition
        /// </summary>
        public ViewDefinition ViewDefinition { get; set; }

        /// <summary>
        /// Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets the detailled element, whose content is shown in the dialog
        /// </summary>
        public IObject DetailElement { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether new properities may be added by the user to the element
        /// </summary>
        public bool AllowNewProperties { get; set; }

        private bool? _hideViewSelection;

        /// <summary>
        /// This event handler is thrown, when the user clicks on 'Save' and after the properties are
        /// transferred from form display to element
        /// </summary>
        public event EventHandler ElementSaved;

        private int _fieldCount;

        /// <summary>
        /// Gets or sets the flag whether design shall be minimized
        /// </summary>
        /// <returns>true, if design shall be minimized</returns>
        public bool IsDesignMinimized()
        {
            return DotNetHelper.IsTrue(ActualFormDefinition?.getOrDefault(_FormAndFields._Form.minimizeDesign));
        }

        /// <summary>
        /// Stores the list of actions that will be performed when the user clicks on set
        /// </summary>
        public List<Action> SetActions { get; }= new List<Action>();
        
        public DetailFormControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the content for a new object
        /// </summary>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IElement formDefinition)
        {
            SetContent(
                InMemoryObject.CreateEmpty(),
                formDefinition);
        }

        public void SetContent(IObject element, IElement formDefinition)
        {
            DetailElement = element ?? InMemoryObject.CreateEmpty();

            ViewDefinition = new ViewDefinition(
                null,
                formDefinition,
                formDefinition == null ? ViewDefinitionMode.Default : ViewDefinitionMode.Specific
            );

            UpdateViewList();
            UpdateContent();
        }

        /// <summary>
        /// This method gets called to update the views
        /// </summary>
        private void UpdateViewList()
        {
            // Skip, if view selection shall be hidden
            if (_hideViewSelection == true) return;

            // Update view
            var views = GetFormsForView()?.ToList();

            if (views != null)
            {
                if (ViewDefinition.Element != null && views?.IndexOf(ViewDefinition.Element) == -1)
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
        /// Gets the enumeration of all views that may match to the shown items
        /// </summary>
        public IEnumerable<IElement> GetFormsForView()
        {
            return App.Scope?.Resolve<IViewFinder>().FindViews((DetailElement as IHasExtent)?.Extent as IUriExtent, DetailElement);
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
        /// This method gets called, when a new item is added or an existing item was modified. 
        /// Derivation of the class which have automatic creation of columns may include additional columns
        /// </summary>
        private void RefreshViewDefinition()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            if (ViewDefinition.Mode == ViewDefinitionMode.Default)
            {
                ActualFormDefinition = viewFinder.FindView(DetailElement);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.AllProperties
                || ViewDefinition.Mode == ViewDefinitionMode.Default && ActualFormDefinition == null)
            {
                ActualFormDefinition = viewFinder.CreateView(DetailElement);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.Specific)
            {
                ActualFormDefinition = ViewDefinition.Element;
            }

            if (_hideViewSelection == null)
            {
                if (DotNetHelper.IsTrue(ActualFormDefinition.getOrDefault(_FormAndFields._Form.fixView)))
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

        /// <summary>
        /// Updates the content
        /// </summary>
        private void UpdateContent()
        {
            RefreshViewDefinition();
            
            DataGrid.Children.Clear();
            SetActions.Clear();
            if (!(ActualFormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            _fieldCount = 0;

            CreateRows(fields);

            if (DetailElement != null)
            {
                var mofElement = (MofElement)DetailElement;
                var uriExtent = mofElement.Extent as MofUriExtent;

                if (!DotNetHelper.IsTrue(ActualFormDefinition.getOrDefault(_FormAndFields._Form.hideMetaClass)))
                {
                    CreateSeparator();

                    var uriExtentText = uriExtent?.contextURI() ?? string.Empty;
                    var fullName = NamedElementMethods.GetFullName(DetailElement);
                    CreateRowForField("Extent:", uriExtentText, true);
                    CreateRowForField("Full Name:", fullName, true);
                    CreateRowForField("Url w/ ID:", mofElement.GetUri(), true);
                    CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

                    // Sets the metaclass
                    if (DotNetHelper.IsFalseOrNotSet(ActualFormDefinition, _FormAndFields._Form.hideMetaClass))
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
        /// Creates the rows
        /// </summary>
        /// <param name="fields">Fields to be created</param>
        private void CreateRows(IReflectiveCollection fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            foreach (var field in fields.Cast<IElement>())
            {
                var fieldType = field.get(_FormAndFields._FieldData.fieldType)?.ToString();
                if (fieldType == MetaClassElementFieldData.FieldType)
                {
                    continue;
                }
                
                var title = field.get(_FormAndFields._FieldData.title).ToString();
                var isReadOnly = field.get(_FormAndFields._FieldData.isReadOnly).ToString();

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = $"{title}: ",
                    IsEnabled = !DotNetHelper.AsBoolean(isReadOnly)
                };

                var contentBlock = FieldFactory.GetUIElementFor(DetailElement, field, this);
                Grid.SetColumn(contentBlock, 1);
                Grid.SetRow(contentBlock, _fieldCount);
                
                CreateRowForField(titleBlock, contentBlock);
            }
        }

        /// <summary>
        /// Creates a row by having a text
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
        /// Creates a new row in the detail view 
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

        private void CreateSeparator()
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());
            var line = new Canvas
            {
                Background = Brushes.DimGray,
                Height = 1.0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0,10,0,10)
            };

            Grid.SetRow(line, _fieldCount);
            Grid.SetColumnSpan(line, 3);

            DataGrid.Children.Add(line);

            _fieldCount++;
        }

        /// <summary>
        /// Adds the default cancel and save buttons
        /// </summary>
        public void AddDefaultButtons(string saveText = "Save")
        {
            if (AllowNewProperties)
            {
                AddGenericButton("New Property", () =>
                {
                    var fieldKey = new TextBox();
                    var fieldValue = new TextBox();

                    SetActions.Add(() =>
                    {
                        var propertyKey = fieldKey.Text;
                        var propertyValue = fieldValue.Text;

                        if (!string.IsNullOrEmpty(propertyKey))
                        {
                            DetailElement.set(propertyKey, propertyValue);
                        }
                    });

                    CreateRowForField(fieldKey, fieldValue);
                    fieldKey.Focus();
                });
            }

            AddGenericButton("Cancel", () =>
            {
                var window = Window.GetWindow(this);
                window?.Close();
            });

            AddGenericButton(saveText, () =>
            {
                try
                {
                    foreach (var action in SetActions)
                    {
                        action();
                    }

                    OnElementSaved();
                    Window.GetWindow(this)?.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            });
        }

        /// <summary>
        /// Adds a button to the view
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

        /// <summary>
        /// Called before the navigation is called
        /// </summary>
        public void PrepareNavigation()
        {
            void ViewConfig()
            {
                var dlg = new ItemXmlViewWindow
                {
                    SupportWriting = true,
                    Owner = Window.GetWindow(this)
                };
                dlg.UpdateContent(ActualFormDefinition);

                dlg.UpdateButtonPressed += (x, y) =>
                {
                    var temporaryExtent = InMemoryProvider.TemporaryExtent;
                    var factory = new MofFactory(temporaryExtent);
                    ActualFormDefinition = dlg.GetCurrentContentAsMof(factory);
                    UpdateContent();
                };

                dlg.ShowDialog();
            }

            NavigationHost.AddNavigationButton(
                "View-Configuration",
                ViewConfig,
                null,
                NavigationCategories.File + ".Views");
        }
    }
}
