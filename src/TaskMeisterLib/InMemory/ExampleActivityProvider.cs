using System;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using TaskMeister.Model;

namespace TaskMeister.InMemory
{
    public class ExampleActivityProvider : IActivityProvider
    {
        private readonly IUriExtent _uriExtent;

        private readonly IFactory _factory;

        private readonly _TaskMeisterModel _model;

        public ExampleActivityProvider(IUriExtent dataExtent, _TaskMeisterModel task, IFactory factory)
        {
            if (dataExtent == null) throw new ArgumentNullException(nameof(dataExtent));
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _uriExtent = dataExtent;
            _factory = factory;
            _model = task;
        }

        public async Task Init()
        {
            if ((await GetAll()).size() == 0)
            {
                var activity1 = await CreateActivity();
                activity1.set(_TaskMeisterModel._IActivity.Name, "Do your homework");
                activity1.set(_TaskMeisterModel._IActivity.Description, "You have to do your homework, otherwise you are out");
                activity1.set(_TaskMeisterModel._IActivity.Created, DateTime.Now.Subtract(TimeSpan.FromDays(1)));
                await AddActivity(activity1);

                var activity2 = await CreateActivity();
                activity2.set(_TaskMeisterModel._IActivity.Name, "Preparation");
                activity2.set(_TaskMeisterModel._IActivity.Description, "Ok, do it");
                activity2.set(_TaskMeisterModel._IActivity.Created, DateTime.Now);
                await AddActivity(activity2);
            }
        }

        /// <summary>
        /// Creates a new element
        /// </summary>
        /// <returns></returns>
        public Task<IElement> CreateActivity()
        {
            return Task.Run(() => _factory.create(_model.__IActivity));
        }

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="activity">Activity to be added</param>
        /// <returns>The created activity</returns>
        public Task AddActivity(IObject activity)
        {
            return Task.Run(() => { _uriExtent.elements().add(activity); });
        }

        public Task<IReflectiveCollection> GetAll()
        {
            return Task.Run(() => _uriExtent.elements() as IReflectiveCollection);
        }

        public Task<IReflectiveCollection> GetAllOpen()
        {
            return Task.Run(() =>
                _uriExtent.elements()
                    .WhenPropertyIsOneOf(
                        _TaskMeisterModel._IActivity.State,
                        new[] { string.Empty, "New", "Inwork"}));
        }
    }
}