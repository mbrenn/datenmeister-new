using System;
using System.IO;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ListViewControl, INavigationGuest
    {
        private string _workspaceId;

        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(string workspaceId)
        {
            _workspaceId = workspaceId;
            var viewExtent = App.Scope.Resolve<ViewLogic>().GetViewExtent();
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(App.Scope);
            var workspace = workspaceExtent.elements().WhenPropertyIs("id", workspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetContent(
                extents, 
                NamedElementMethods.GetByFullName(viewExtent, ManagementViewDefinitions.PathExtentListView));

            AddDefaultButtons();
            AddRowItemButton("Show Items", ShowItems);
            
            void ShowItems(IObject extentElement)
            {
                var uri = extentElement.get("uri").ToString();

                var events = Navigator.TheNavigator.NavigateToItemsInExtent(
                    NavigationHost,
                    workspaceId,
                    uri);
                events.Closed += (x, y) => UpdateContent();
            }
        }

        /// <summary>
        /// Adds the navigation control elements in the host
        /// </summary>
        public new void PrepareNavigation()
        {
            NavigationHost.AddNavigationButton(
                "New Xmi Extent", 
                NewXmiExtent, 
                null, 
                NavigationCategories.File + ".Workspaces");

            NavigationHost.AddNavigationButton(
                "Zip-Code Example",
                AddZipCodeExample,
                null,
                NavigationCategories.File + ".Workspaces");

            base.PrepareNavigation();

            void NewXmiExtent()
            {
                var events = Navigator.TheNavigator.NavigateToNewXmiExtentDetailView(NavigationHost, _workspaceId);
                events.Closed += (x, y) => UpdateContent();
            }

            void AddZipCodeExample()
            {
                var extentManager = App.Scope.Resolve<IExtentManager>();

                var random = new Random();

                // Finds the file and copies the file to the given location
                var appBase = AppContext.BaseDirectory;

                // Creates directory, if it does not exist
                var directory = Path.Combine(appBase, "App_Data/Database");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string filename;
                var tries = 0;
                int randomNumber;
                do // while File.Exists
                {
                    randomNumber = random.Next(int.MaxValue);
                    filename = Path.Combine(appBase, "App_Data/Database", $"plz_{randomNumber}.csv");
                    tries++;
                    if (tries == 10000)
                    {
                        throw new InvalidOperationException("Did not find a unique name for zip extent");
                    }
                } while (File.Exists(filename));

                var originalFilename = Path.Combine(appBase, "Examples", "plz.csv");

                File.Copy(originalFilename, filename);

                var defaultConfiguration = new CSVExtentLoaderConfig
                {
                    ExtentUri = $"datenmeister:///zipcodes/{randomNumber}",
                    Path = filename,
                    Workspace = _workspaceId,
                    Settings =
                    {
                        HasHeader = false,
                        Separator = '\t',
                        Encoding = "UTF-8",
                        Columns = new[] {"Id", "Zip", "PositionLong", "PositionLat", "CityName"}.ToList(),
                        MetaclassUri = "dm:///types#DatenMeister.Apps.ZipCode.Model.ZipCode"
                    }
                };

                extentManager.LoadExtent(defaultConfiguration, false);

                UpdateContent();
            }
        }
    }
}