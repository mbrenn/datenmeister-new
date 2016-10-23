using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace TaskMeister
{
    /// <summary>
    /// Defines the interface that is necessary to work on the tasks
    /// </summary>
    public interface IActivityProvider
    {
        /// <summary>
        /// Adds an activity to the 
        /// </summary>
        /// <returns>The Task for adding the activity</returns>
        Task<IElement> CreateActivity();

        /// <summary>
        /// Gets all activities of the database
        /// </summary>
        /// <returns>The task of getting the activities as a local, cached instance</returns>
        Task<IReflectiveCollection> GetAll();

        /// <summary>
        /// Gets all open activities of the database
        /// </summary>
        /// <returns>The task of getting the activities as a local, cached instance</returns>
        Task<IReflectiveCollection> GetAllOpen();

        /// <summary>
        /// Adds an activity to the taskmeister
        /// </summary>
        /// <param name="activity">Activity to be added</param>
        /// <returns>The task, which is adding the activity</returns>
        Task AddActivity(IObject activity);
    }
}