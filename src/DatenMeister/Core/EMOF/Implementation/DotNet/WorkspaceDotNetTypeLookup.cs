using System;
using System.Collections.Generic;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Core.EMOF.Implementation.DotNet
{
    /// <summary>
    /// Performs an abstraction of the workspace to have an access to the types of the extents
    /// </summary>
    public class WorkspaceDotNetTypeLookup : IDotNetTypeLookup
    {
        /// <summary>
        /// Defines a cache between all objects and their id
        /// </summary>
        private readonly Dictionary<object, string> _cacheObjectToId =
            new Dictionary<object, string>();
        
        private readonly Workspace _workspace;

        public WorkspaceDotNetTypeLookup(Workspace workspace)
        {
            _workspace = workspace;
        }
        public void Add(string metaclassUri, Type type)
        {
            throw new NotImplementedException();
        }

        public string? ToElement(Type type)
        {
            return WorkspaceDotNetHelper.GetMetaClassUriOfDotNetType(_workspace, type);
        }

        public Type? ToType(string metaclassUri)
        {
            return WorkspaceDotNetHelper.GetDotNetTypeOfMetaClassUri(_workspace, metaclassUri);
        }

        /// <summary>
        /// Gets the id of a certain metaclassUri
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>The returned id</returns>
        public string GetId(object value)
        {
            lock (_cacheObjectToId)
            {
                if (!_cacheObjectToId.TryGetValue(value, out var id))
                {
                    id = Guid.NewGuid().ToString();
                    _cacheObjectToId[value] = id;
                }

                return id;
            }
        }
    }
}