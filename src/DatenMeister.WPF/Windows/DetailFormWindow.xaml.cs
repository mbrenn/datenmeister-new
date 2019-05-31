﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Copier;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    ///     Interaktionslogik für DetailFormWindow.xaml
    /// </summary>
    public partial class DetailFormWindow : Window, IDetailNavigationHost
    {
        /// <summary>
        /// Defines the logger for the DetailFormWindow
        /// </summary>
        private static readonly ClassLogger Logger = new ClassLogger(typeof(DetailFormWindow));

        /// <summary>
        /// Defines the event that will be thrown, when the user has clicked upon 'save' in the inner form.
        /// This event has to be invoked by the child elements. 
        /// </summary>
        public event EventHandler<ItemEventArgs> Saved;

        /// <summary>
        /// Defines the event that will be thrown, when the user has clicked upon 'save' in the inner form.
        /// This event has to be invoked by the child elements. 
        /// </summary>
        public event EventHandler<ItemEventArgs> Cancelled;

        /// <summary>
        /// Stores the value whether the  saved or cancelled event is already thrown. 
        /// This information is used to decide whether to throw an event when user closes the dialogue
        /// </summary>
        private bool _finalEventsThrown;

        /// <summary>
        /// Stores the detail element
        /// </summary>
        private ViewDefinition _viewDefinition;

        /// <summary>
        /// Stores the effective form
        /// </summary>
        public IElement EffectiveForm { get; private set; }

        /// <summary>
        /// Stores the form element
        /// </summary>
        public IObject DetailElement { get; private set; }

        /// <summary>
        /// Stores the attached element
        /// </summary>
        public IElement AttachedElement { get; private set; }

        /// <summary>
        /// Gets or sets the container element in which the DetailElement is allocated to.
        /// This information is used to delete the item if required
        /// </summary>
        public IReflectiveCollection DetailElementContainer { get; set; }

        /// <summary>
        /// Gets a helper for menu
        /// </summary>
        private MenuHelper MenuHelper { get; }

        /// <summary>
        /// Initializes a new instance of the DetailFormWindow class. 
        /// </summary>
        public DetailFormWindow()
        {
            InitializeComponent();
            MenuHelper = new MenuHelper(MainMenu);
        }

        /// <summary>
        /// Gets the ui element of the main control
        /// </summary>
        public UIElement MainControl => MainContent.Content as UIElement;

        public Task<NavigateToElementDetailResult> NavigateTo(Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            return Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
        }

        /// <summary>
        ///     Rebuild the complete navigation
        /// </summary>
        public void RebuildNavigation()
        {
            var detailForm = (DetailFormControl) MainContent?.Content;
            var extensions = GetDefaultExtension();
            var otherExtensions = detailForm?.GetViewExtensions();
            if (otherExtensions != null)
            {
                extensions = extensions.Union(otherExtensions);
            }

            var extensionList = extensions.ToList();
            detailForm?.EvaluateViewExtensions(extensionList);
            MenuHelper.EvaluateExtensions(extensionList);
        }

        /// <summary>
        /// Gets the default extensions
        /// </summary>
        /// <returns>Enumeration of extensions</returns>
        private IEnumerable<ViewExtension> GetDefaultExtension()
        {
            yield return new RibbonButtonDefinition(
                "Close",
                CloseWindow,
                null,
                NavigationCategories.File,
                -10);

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
            {
                Focus();
            }
            else
            {
                MainContent.Focus();
            }
        }

        /// <inheritdoc />
        public Window GetWindow()
        {
            return this;
        }

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
                {
                    Title = $"Edit element: {name}";
                }
            }

            if (element is IHasTitle title)
            {
                Title = title.Title;
            }
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
            var height = control.EffectiveForm.getOrDefault<double>(_FormAndFields._DetailForm.defaultWidth);
            if (width <= 0 && height <= 0)
            {
                width = 1000;
                height = 1000;
            }

            MainMenu.Measure(new Size(width,height));
            var heightOffset = MainMenu.DesiredSize.Height;

            control.Measure(new Size(width, height - heightOffset));
            Width = Math.Ceiling(control.DesiredSize.Width) + 50;
            Height = Math.Ceiling(control.DesiredSize.Height) + 50 + heightOffset;
        }

        public void OnSaved(IObject detailElement, IObject attachedElement)
        {
            if (_finalEventsThrown)
            {
                throw new InvalidOperationException("Final event was already thrown");
            }

            _finalEventsThrown = true;
            Saved?.Invoke(this, 
                new ItemEventArgs(
                detailElement, 
                attachedElement));
        }

        public void OnCancelled(IObject detailElement, IObject attachedElement)
        {
            if (_finalEventsThrown)
            {
                throw new InvalidOperationException("Final event was already thrown");
            }

            _finalEventsThrown = true;
            Cancelled?.Invoke(this,
                new ItemEventArgs(
                    detailElement,
                    attachedElement));
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
        /// <param name="viewDefinition"></param>
        public void SetContent(IObject element, ViewDefinition viewDefinition, IReflectiveCollection container = null)
        {
            DetailElement = element ?? InMemoryObject.CreateEmpty();
            DetailElementContainer = container;
            _viewDefinition = viewDefinition;
            
            UpdateActualViewDefinition();

            CreateDetailForm();
        }

        /// <summary>
        /// Sets the form and updates the content of the detail form
        /// </summary>
        /// <param name="formDefinition"></param>
        public void SetForm(IElement formDefinition)
        {
            _viewDefinition = new ViewDefinition(formDefinition);
            UpdateActualViewDefinition();
            CreateDetailForm();
        }

        /// <summary>
        ///     Sets the content for a completely new object
        /// </summary>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IElement formDefinition)
        {
            SetContent(
                InMemoryObject.CreateEmpty(),
                new ViewDefinition(formDefinition));
        }

        /// <summary>
        /// Creates the detailform matching to the given effective form
        /// </summary>
        private void CreateDetailForm()
        {
            if (EffectiveForm != null)
            {
                var control = new DetailFormControl
                {
                    AllowNewProperties = true,
                    NavigationHost = this,
                    EffectiveForm = EffectiveForm
                };
                
                control.UpdateContent();
                SetMainContent(control);

                var title = EffectiveForm.getOrDefault<string>(_FormAndFields._DetailForm.title);
                if (!string.IsNullOrEmpty(title))
                {
                    Title = title;
                }
            }
        }
        
        /// <summary>
        /// Sets the effective form by using the viewdefinition
        /// </summary>
        private void UpdateActualViewDefinition()
        {
            var viewFinder = GiveMe.Scope.Resolve<ViewFinderImpl>();
            if (_viewDefinition.Mode == ViewDefinitionMode.Default)
            {
                EffectiveForm = viewFinder.FindDetailView(DetailElement);
            }

            switch (_viewDefinition.Mode)
            {
                case ViewDefinitionMode.AllProperties:
                case ViewDefinitionMode.Default when EffectiveForm == null:
                    EffectiveForm = viewFinder.CreateView(DetailElement);
                    break;
                case ViewDefinitionMode.Specific:
                    EffectiveForm = _viewDefinition.Element;
                    break;
            }

            // Clones the EffectiveForm
            EffectiveForm = ObjectCopier.Copy(new MofFactory(EffectiveForm), EffectiveForm);
        }
    }
}