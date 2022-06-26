using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Users.UserProperties
{
    /// <summary>
    /// This class stores all the user properties belonging to a certain user. This is the aggregation
    /// of all possible property types that can be attached to a user's profile
    /// </summary>
    public class UserPropertyData
    {
        private List<ViewModeSelection> _viewModeSelection = new();

        /// <summary>
        /// Adds an additional view mode to the selection
        /// </summary>
        /// <param name="extentUri">Uri of the extent to be considered</param>
        /// <param name="viewMode">The viewmode being evaluated</param>
        /// <param name="tag">The tag that may be associated</param>
        public void AddViewModeSelection(string extentUri, IElement viewMode, string? tag = null)
        {
            lock (_viewModeSelection)
            {
                var foundViewMode = Get(extentUri, tag);
                if (foundViewMode == null)
                {
                    var newViewMode = new ViewModeSelection
                    {
                        tag = tag,
                        viewMode = viewMode,
                        extentUri = extentUri
                    };

                    _viewModeSelection.Add(newViewMode);
                }
                else
                {
                    foundViewMode.viewMode = viewMode;
                }
            }
        }


        /// <summary>
        /// Gets the first view mode of the user 
        /// </summary>
        /// <param name="extentUri">Uri of the extent</param>
        /// <param name="tag">Tag to be used</param>
        /// <returns>The found viewmode selection</returns>
        public IElement? GetViewModeSelection(string extentUri, string? tag = null)
        {
            return Get(extentUri, tag)?.viewMode;
        }

        /// <summary>
        /// Gets the first view mode of the user 
        /// </summary>
        /// <param name="extentUri">Uri of the extent</param>
        /// <param name="tag">Tag to be used</param>
        /// <returns>The found viewmode selection</returns>
        private ViewModeSelection? Get(string extentUri, string? tag = null)
        {
            lock (_viewModeSelection)
            {
                return _viewModeSelection.FirstOrDefault(
                    x => x.extentUri == extentUri
                         && (tag == null || x.tag == tag));
            }
        }
    }
}