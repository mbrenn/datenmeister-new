using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core
{
    public static class WorkspaceExtension
    {
        public static IObject FindElementByUri(this Workspace workspace, string uri)
        {
            foreach (var extent in workspace.extent)
            {
                var extentAsUriExtent = extent as IUriExtent;
                var result = extentAsUriExtent?.element(uri);
                if (result != null)
                {
                    // found it
                    return result;
                }
            }

            // Not found
            return null;
        }

        public static IObject FindElementByUri(this IEnumerable<IUriExtent> extents, string uri)
        {
            foreach (var extent in extents)
            {
                var extentAsUriExtent = extent as IUriExtent;
                var result = extentAsUriExtent?.element(uri);
                if (result != null)
                {
                    // found it
                    return result;
                }
            }

            // Not found
            return null;
        }

        public static Workspace FindWorkspace(this IEnumerable<Workspace> workspaces, IUriExtent extent)
        {
            return workspaces.FirstOrDefault(x => x.extent.Contains(extent));
        }

        /// <summary>
        /// Gets the metalayer
        /// </summary>
        /// <param name="logic">Datalayer being queried</param>
        /// <param name="value">Datalayer for the value</param>
        /// <returns>The metalayer ot the object</returns>
        public static Workspace GetMetaLayerOfObject(this IWorkspaceLogic logic, IObject value) 
        {
            var dataLayer = logic.GetDataLayerOfObject(value);
            if (dataLayer == null)
            {
                return null;
            }

            return logic.GetMetaLayerFor(dataLayer);
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="dataLayer">The datalayer that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IWorkspaceLogic logic,
            Workspace dataLayer)
            where TFilledType : class, new()
        {
            var metaLayer = logic.GetMetaLayerFor(dataLayer);
            return metaLayer != null ? logic.Get<TFilledType>(metaLayer) : null;
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="extent">The extent that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IWorkspaceLogic logic,
            IExtent extent)
            where TFilledType : class, new()
        {
            var dataLayer = logic.GetDataLayerOfExtent(extent);
            if (dataLayer == null)
            {
                return null;
            }

            return GetFromMetaLayer<TFilledType>(logic, dataLayer);
        }
    }
}