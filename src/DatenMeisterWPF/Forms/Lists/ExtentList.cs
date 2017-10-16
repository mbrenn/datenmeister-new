using System;
using System.IO;
using System.Linq;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Lists
{
    public class ExtentList : ElementListViewControl
    {
        /// <summary>
        /// Shows the workspaces of the DatenMeister
        /// </summary>
        /// <param name="scope">Scope of the DatenMeister</param>
        /// <param name="workspaceId">Id of the workspace whose extents shall be shown</param>
        public void SetContent(IDatenMeisterScope scope, string workspaceId)
        {
            var viewExtent = scope.Resolve<ViewLogic>().GetViewExtent();
            var workspaceExtent = ManagementProviderHelper.GetExtentsForWorkspaces(scope);
            var workspace = workspaceExtent.elements().WhenPropertyIs("id", workspaceId).FirstOrDefault() as IElement;

            var extents = workspace?.get("extents") as IReflectiveSequence;
            SetContent(
                scope, 
                extents, 
                NamedElementMethods.GetByFullName(viewExtent, ViewDefinitions.PathExtentListView));

            AddDefaultButtons();
            AddGenericButton("New Xmi Extent", NewXmiExtent);
            AddRowItemButton("Show Items", ShowItems);
            AddGenericButton("Zip-Code Example", AddZipCodeExample);


            void NewXmiExtent()
            {
                var window = Window.GetWindow(this);
                var events = Navigator.TheNavigator.NavigateToNewXmiExtentDetailView(window, scope, workspaceId);
                events.Closed += (x, y) => UpdateContent();
            }

            void ShowItems(IObject extentElement)
            {
                var window = Window.GetWindow(this);
                Navigator.TheNavigator.NavigateTo(window, () =>
                {
                    var workLogic = scope.Resolve<IWorkspaceLogic>();
                    var uri = extentElement.get("uri").ToString();
                    var extent = workLogic.FindExtent(workspaceId, uri);
                    if (extent == null)
                    {
                        return null;
                    }


                    var control = new ElementListViewControl();
                    control.SetContent(scope, extent.elements(), null);
                    control.AddDefaultButtons();

                    return control;
                });
            }

            void AddZipCodeExample()
            {
                var extentManager = scope.Resolve<IExtentManager>();

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
                    Workspace = workspaceId,
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