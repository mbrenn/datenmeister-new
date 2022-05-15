using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.TemporaryExtent
{
    /// <summary>
    /// Defines some helper methods for the temporary extent plugin
    /// </summary>
    public class TemporaryExtentLogic
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public TemporaryExtentLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Creates a simple temporary element
        /// </summary>
        /// <param name="metaClass">Metaclass to be used</param>
        /// <returns>The created element itself</returns>
        public IElement CreateTemporaryElement(IElement? metaClass)
        {
            var foundExtent =
                _workspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, TemporaryExtentPlugin.Uri)
                ?? throw new InvalidOperationException("The temporary extent was not found");

            var created = MofFactory.Create(foundExtent, metaClass);
            foundExtent.elements().add(created);

            return created;
        }
    }
}