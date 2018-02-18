using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeisterWPF.Forms.Base;

namespace DatenMeisterWPF.Navigation
{
    public class CreatableTypeNavigator : ControlNavigation
    {
        /// <summary>
        /// Sets the selected types
        /// </summary>
        public IElement SelectedType { get; private set; }

        /// <summary>
        /// Shows a dialog in which the user can select one type out of the list of createable types
        /// </summary>
        /// <param name="window">Window to tbe used</param>
        /// <param name="extent">Extent to whom the type shall be created</param>
        /// <param name="buttonName">Name of the button</param>
        /// <returns>The control navigation</returns>
        public IControlNavigation NavigateToSelectCreateableType(INavigationHost window, IExtent extent, string buttonName = "Create")
        {
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var viewDefinitions = App.Scope.Resolve<ManagementViewDefinitions>();
            var extentFunctions = App.Scope.Resolve<ExtentFunctions>();
            
            return window.NavigateTo(
                () =>
                {
                    var element = InMemoryObject.CreateEmpty().SetReferencedExtent(viewLogic.GetViewExtent());
                    var items = extentFunctions.GetCreatableTypes(extent).CreatableTypes;
                    var formPathToType = viewDefinitions.GetFindTypeForm(items);

                    var control = new DetailFormControl();
                    control.SetContent(element, formPathToType);
                    control.AddDefaultButtons(buttonName);
                    control.ElementSaved += (x, y) =>
                    {
                        if (control.DetailElement.getOrDefault("selectedType") is IElement metaClass)
                        {
                            SelectedType = metaClass;
                        }

                        OnClosed();
                    };

                    return control;
                },
                NavigationMode.ForceNewWindow);
        }
    }
}