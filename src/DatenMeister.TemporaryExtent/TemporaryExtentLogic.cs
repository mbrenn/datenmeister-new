using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.TemporaryExtent
{
    /// <summary>
    /// Defines some helper methods for the temporary extent plugin
    /// </summary>
    public class TemporaryExtentLogic
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(TemporaryExtentLogic));
        
        private readonly IWorkspaceLogic _workspaceLogic;
        
        public static TimeSpan DefaultCleanupTime { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// Defines the default cleanup time name
        /// </summary>
        public const string CleanUpTimeFieldName = "_toBeCleanedUp";

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
            created.set(CleanUpTimeFieldName, DateTime.Now + DefaultCleanupTime);
            foundExtent.elements().add(created);

            return created;
        }

        public void CleanElements()
        {
            var foundExtent =
                _workspaceLogic.FindExtent(WorkspaceNames.WorkspaceData, TemporaryExtentPlugin.Uri)
                ?? throw new InvalidOperationException("The temporary extent was not found");

            var currentTime = DateTime.Now;

            // Go through the elements and collect these ones whose clean up time has passed
            var itemsToBeDeleted = 
                foundExtent.elements()
                    .OfType<IObject>()
                    .Where(element => element.getOrDefault<DateTime>(CleanUpTimeFieldName) < currentTime)
                    .ToList();

            // Now delete these items
            foreach (var element in itemsToBeDeleted)
            {
                foundExtent.elements().remove(element);
            }

            // Logging, if something was deleted
            if (itemsToBeDeleted.Count > 0)
            {
                ClassLogger.Info($"{itemsToBeDeleted.Count} items deleted");
            }
        }
    }
}