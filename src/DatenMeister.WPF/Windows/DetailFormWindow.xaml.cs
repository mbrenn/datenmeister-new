#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.Validators;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
using DatenMeister.WPF.Navigation;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    ///     Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, INavigationHost
    {
        /// <summary>
        /// Defines the logger for the DetailFormWindow
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DetailFormWindow));

        /// <summary>
        /// Defines the event that will be thrown, when the user has clicked upon 'save' in the inner form.
        /// This event has to be invoked by the child elements.
        /// </summary>
        public event EventHandler<ItemEventArgs>? Saved;

        /// <summary>
        /// Defines the event that will be thrown, when the user has clicked upon 'save' in the inner form.
        /// This event has to be invoked by the child elements.
        /// </summary>
        public event EventHandler<ItemEventArgs>? Cancelled;

        /// <summary>
        /// Stores the value whether the saved or cancelled event is already thrown.
        /// This information is used to decide whether to throw an event when user closes the dialogue
        /// </summary>
        private bool _finalEventsThrown;

        /// <summary>
        /// Stores the view definition as requested by the creator of the window.
        /// This view definition may be overridden by the OverridingViewDefinition
        /// </summary>
        private FormDefinition? _requestedFormDefinition;

        /// <summary>
        /// Gets a helper for menu
        /// </summary>
        private MenuHelper MenuHelper { get; }
        
        public IObject? DetailElement { get; set; }
        
        /// <summary>
        /// Stores the collection being used as a container
        /// </summary>
        public IReflectiveCollection? ContainerCollection { get; set; }

        /// <summary>
        /// Gets or sets the form that is overriding the default form
        /// </summary>
        public FormDefinition? OverridingFormDefinition { get; private set; }

        /// <summary>
        /// Sets the form that shall be shown instead of the default form as created by the inheriting items
        /// </summary>
        /// <param name="form"></param>
        public void SetOverridingForm(IElement form)
        {
            OverridingFormDefinition = new FormDefinition(form);
            RecreateView();
        }

        /// <summary>
        /// Clears the overriding form, so the default views are used 
        /// </summary>
        public void ClearOverridingForm()
        {
            OverridingFormDefinition = null;
            RecreateView();
        }

        public IEnumerable<IObject> GetSelectedItems()
        {
            var detailElement = DetailElement;

            if (detailElement == null)
            {
                yield break;
            }
            
            yield return detailElement;
        }

        /// <summary>
        /// Initializes a new instance of the DetailFormWindow class.
        /// </summary>
        public DetailFormWindow()
        {
            InitializeComponent();
            MenuHelper = new MenuHelper(MainMenu, NavigationScope.Item)
            {
                ShowApplicationItems = false
            };
        }

        /// <summary>
        /// Gets the ui element of the main control
        /// </summary>
        public UIElement? MainControl => MainContent.Content as UIElement;

        public async Task<NavigateToElementDetailResult?> NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
            => await Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);

        /// <summary>
        /// Rebuilds the navigation by going through all view extensions
        /// </summary>
        public void RebuildNavigation()
        {
            var navigationGuest = MainContent?.Content as INavigationGuest;

            // 1) Ask myself
            var extensions = GetDefaultExtension().ToList();

            // 2) Ask the navigation guest
            var otherExtensions = navigationGuest?.GetViewExtensions();
            if (otherExtensions != null)
            {
                extensions.AddRange(otherExtensions);
            }

            // 3) Ask the plugin
            var viewExtensionPlugins = GuiObjectCollection.TheOne.ViewExtensionFactories;
            var data = new ViewExtensionTargetInformation()
            {
                NavigationGuest = navigationGuest,
                NavigationHost = this
            };

            extensions.AddRange(
                viewExtensionPlugins.SelectMany(
                    plugin => plugin.GetViewExtensions(data)));

            // Now navigation guest and the window itself can work upon the view extensions
            // The navigation guest gets a chance for the views
            var extensionList = extensions.ToList();
            navigationGuest?.EvaluateViewExtensions(extensionList);

            // The menuhelper itself is asked to work upon the view
            MenuHelper.Item = (navigationGuest as IItemNavigationGuest)?.Item;
            MenuHelper.ShowApplicationItems = true;
            MenuHelper.NavigationScope = NavigationScope.Application | NavigationScope.Item;
            MenuHelper.EvaluateExtensions(extensionList);
        }

        /// <summary>
        /// Gets the default extensions
        /// </summary>
        /// <returns>Enumeration of extensions</returns>
        private IEnumerable<ViewExtension> GetDefaultExtension()
        {
            yield return new ApplicationMenuButtonDefinition(
                "Close",
                CloseWindow,
                null,
                NavigationCategories.DatenMeister,
                2000);

            yield return new ItemButtonDefinition(
                "Cancel",
                x => CloseWindow());

            // Local methods for the buttons
            void CloseWindow()
            {
                Close();
            }
        }

        /// <summary>
        /// Sets the focus of the detail form
        /// </summary>
        public void SetFocus()
        {
            if (MainContent == null)
                Focus();
            else
                MainContent.Focus();
        }

        /// <inheritdoc />
        public Window GetWindow() => this;

        private void DetailFormWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            RebuildNavigation();
        }

        /// <summary>
        ///     Sets the main content to be shown
        /// </summary>
        /// <param name="element">Element to be shown</param>
        public void SetMainContent(UIElement element)
        {
            MainContent.Content = element;
            if (element is DetailFormControl control)
            {
                var size = control.DefaultSize;
                if (Math.Abs(size.Width) > 1E-7 && size.Height > 1E-7)
                {
                    var window = GetWindow(this);
                    if (window != null)
                    {
                        window.Width = size.Width;
                        window.Height = size.Height;
                    }
                }

                var name = NamedElementMethods.GetName(control.DetailElement);
                if (!string.IsNullOrEmpty(name))
                    Title = $"Edit element: {name}";
            }

            if (element is IHasTitle title)
                Title = title.Title;
        }

        /// <summary>
        /// Takes the content of the window and resizes the window
        /// to the detail form elements.
        /// </summary>
        public void SwitchToMinimumSize()
        {
            var control = MainContent.Content as DetailFormControl;
            if (control?.EffectiveForm == null)
            {
                return;
            }

            var width = control.EffectiveForm.getOrDefault<double>(_FormAndFields._DetailForm.defaultWidth);
            var height = control.EffectiveForm.getOrDefault<double>(_FormAndFields._DetailForm.defaultHeight);
            if (width <= 0 && height <= 0)
            {
                width = 1000;
                height = 1000;
            }

            MainMenu.Measure(new Size(width, height));
            var heightOffset = MainMenu.DesiredSize.Height;

            control.Measure(new Size(width, height - heightOffset));
            Width = Math.Ceiling(control.DesiredSize.Width) + 50;
            Height = Math.Ceiling(control.DesiredSize.Height) + 50 + heightOffset;
        }

        public void OnSaved(IObject? detailElement, IObject? attachedElement)
        {
            if (_finalEventsThrown)
            {
                throw new InvalidOperationException("Final event was already thrown");
            }

            if (detailElement == null)
                throw new InvalidOperationException("detailElement == null");

            _finalEventsThrown = true;
            Saved?.Invoke(this,
                new ItemEventArgs(
                    detailElement,
                    attachedElement));
        }

        public void OnCancelled(IObject? detailElement, IObject? attachedElement)
        {
            if (_finalEventsThrown)
            {
                throw new InvalidOperationException("Final event was already thrown");
            }

            _finalEventsThrown = true;

            if (detailElement != null)
            {
                Cancelled?.Invoke(this,
                    new ItemEventArgs(
                        detailElement,
                        attachedElement));
            }
        }

        private void DetailFormWindow_OnClosed(object sender, EventArgs e)
        {
            if (!_finalEventsThrown)
            {
                OnCancelled(null, null);
            }
        }

        /// <summary>
        /// Opens the form by creating the inner dialog
        /// </summary>
        /// <param name="element"></param>
        /// <param name="formDefinition"></param>
        /// <param name="container">Container being used when the item is added</param>
        public void SetContent(IObject? element, FormDefinition? formDefinition, IReflectiveCollection? container = null)
        {
            element ??= InMemoryObject.CreateEmpty();
            CreateDetailForm(element, formDefinition, container);
        }

        /// <summary>
        ///     Sets the content for a completely new object
        /// </summary>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IElement formDefinition)
        {
            SetContent(
                InMemoryObject.CreateEmpty(),
                new FormDefinition(formDefinition));
        }

        /// <summary>
        /// Creates the detailform matching to the given effective form as set by the effective Form
        /// </summary>
        private void CreateDetailForm(IObject detailElement, FormDefinition? formDefinition, IReflectiveCollection? container = null)
        {
            DetailElement = detailElement;
            ContainerCollection = container;
            _requestedFormDefinition = formDefinition;
            
            RecreateView();
        }

        private void RecreateView()
        {
            if (DetailElement == null)
                throw new InvalidOperationException("DetailElement == null");

            var formDefinition = _requestedFormDefinition;
            
            IObject? effectiveForm = null;
            var viewLogic = GiveMe.Scope.Resolve<FormLogic>();

            // Checks, if there is an overriding form 
            if (OverridingFormDefinition != null)
            {
                if (OverridingFormDefinition.Mode == FormDefinitionMode.Specific)
                {
                    effectiveForm = OverridingFormDefinition.Element;
                }
                else
                {
                    effectiveForm = viewLogic.GetDetailForm(DetailElement, DetailElement.GetUriExtentOf(), OverridingFormDefinition.Mode);
                }

                formDefinition = OverridingFormDefinition;
            }

            // If not, take the standard procedure
            if (effectiveForm == null)
            {
                formDefinition = 
                    _requestedFormDefinition ??= new FormDefinition(FormDefinitionMode.Default);
                
                if (_requestedFormDefinition.Mode == FormDefinitionMode.Specific)
                {
                    effectiveForm = _requestedFormDefinition.Element ??
                                    throw new InvalidOperationException("_requestedFormDefinition.Element == null");
                }
                else
                {
                    effectiveForm =
                        viewLogic.GetDetailForm(
                            DetailElement,
                            DetailElement.GetUriExtentOf(),
                            _requestedFormDefinition.Mode);
                }
            }

            // Clones the EffectiveForm, so modification are possible afterwards by plugins without changing
            // the original form
            effectiveForm = ObjectCopier.Copy(new MofFactory(effectiveForm), effectiveForm);

            if (effectiveForm != null)
            {
                var control = new DetailFormControl {NavigationHost = this};
                control.SetContent(DetailElement, effectiveForm, ContainerCollection);
                if (formDefinition != null)
                {
                    foreach (var validator in formDefinition.Validators)
                    {
                        control.ElementValidators.Add(validator);
                    }
                }

                control.UpdateView();
                control.ElementSaved += (x, y) =>
                {
                    OnSaved(control.DetailElement, control.AttachedElement);
                    Close();
                };

                control.AddGenericButton("Cancel", () =>
                {
                    OnCancelled(control.DetailElement, control.AttachedElement);
                    Close();
                }).IsCancel = true;

                SetMainContent(control);

                var title = effectiveForm.getOrDefault<string>(_FormAndFields._DetailForm.title);
                if (!string.IsNullOrEmpty(title))
                {
                    Title = title;
                }

                RebuildNavigation();
            }
        }

        private void DetailFormWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}