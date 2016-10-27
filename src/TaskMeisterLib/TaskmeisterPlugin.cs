using System.Diagnostics;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.FactoryMapper;
using DatenMeister.Runtime.Workspaces;
using TaskMeister.Model;

namespace TaskMeister
{
    /// <summary>
    /// Defines the plugin class for the TaskMeister
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class TaskmeisterPlugin : IDatenMeisterPlugin
    {
        private readonly ILifetimeScope _lifetimeScope;

        /// <summary>
        /// Stores the workspace collection
        /// </summary>

        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IFactoryMapper _factoryMapper;
        private readonly IWebserverStartupPhases _webserverStartupPhases;
        private readonly ViewLogic _viewLogic;
        private readonly IDotNetTypeLookup _typeLookup;

        public TaskmeisterPlugin(
            ILifetimeScope lifetimeScope,
            IWorkspaceLogic workspaceLogic,
            IFactoryMapper factoryMapper,
            IWebserverStartupPhases webserverStartupPhases,
            ViewLogic viewLogic,
            IDotNetTypeLookup typeLookup)
        {
            _lifetimeScope = lifetimeScope;
            _workspaceLogic = workspaceLogic;
            _factoryMapper = factoryMapper;
            _webserverStartupPhases = webserverStartupPhases;
            _viewLogic = viewLogic;
            _typeLookup = typeLookup;
        }

        public void Start()
        {
            Debug.WriteLine("Taskmeister - Plugin started");
            var model = new _TaskMeisterModel();

            // Creates the types
            var typeExtent = _workspaceLogic.FindTypeExtent();

            var uml = _workspaceLogic.GetFromMetaLayer<_UML>(typeExtent);
            var factory = _factoryMapper.FindFactoryFor(_lifetimeScope, typeExtent);
            IntegrateTaskMeisterModel.Assign(uml, factory, typeExtent.elements(), model, _typeLookup);

            Debug.WriteLine("TaskMeister - Types added");

            // View for the activity list
            var view = new Form { name = "Activity List" };
            view.fields.Add(new TextFieldData("name", "Name!"));
            _viewLogic.Add(_typeLookup.CreateDotNetElement(view, typeExtent as DotNetExtent, "Views.Activity.Detail"));

            // View for the default list
            var taskDetailView = new Form { name = "Activity Detail" };
            taskDetailView.fields.Add(new TextFieldData(_TaskMeisterModel._IActivity.Name, "Name"));
            taskDetailView.fields.Add(new TextFieldData(_TaskMeisterModel._IActivity.Description, "Description")
            {
                lineHeight = 10
            });
            taskDetailView.fields.Add(new DateTimeFieldData(_TaskMeisterModel._IActivity.StartDate, "Description")
            {
                showDate = true
            });
            
            // Creates the default view
            var date = new DefaultViewForMetaclass
            {
                view = taskDetailView,
                metaclass =  (model.__IActivity as IHasId).Id,
                viewType = ViewType.Detail
            };

            _viewLogic.Add(_typeLookup.CreateDotNetElement(date));

            // Creates default view
            _webserverStartupPhases.AfterInitialization += (x, y) =>
            {
                var plugin = y.Scope.Resolve<IClientModulePlugin>();
                plugin.AddScript("taskmeister");
            };
        }
    }
}