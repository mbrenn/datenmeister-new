using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.ChangeEvents
{
    /// <summary>
    /// This class handles all events occuring due to changes, insertion or deletions of the objects
    /// within the extent
    /// </summary>
    public class ChangeEventManager
    {
        private static readonly ClassLogger ClassLogger = new ClassLogger(typeof(ChangeEventManager));

        /// <summary>
        /// Defines the locking for the change events
        /// </summary>
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Stores the handles that would like to get informed about changes
        /// </summary>
        private readonly List<RegisteredEventHandle> _handles = new List<RegisteredEventHandle>();

        /// <summary>
        /// Has to be called, when the given object has changed
        /// </summary>
        /// <param name="value">Object that was changed</param>
        public void SendChangeEvent(IObject value)
        {
            var extent = value.GetExtentOf();
            var workspace = extent?.GetWorkspace();

            RegisteredEventHandle[] handles;
            try
            {
                _lock.EnterReadLock();
                if (_handles.Count == 0)
                {
                    // Nothing to do.
                    return;
                }
                
                handles = _handles.Where(
                    x => x.Value?.Equals(value) == true
                         || x.Extent?.Equals(extent) == true
                         || x.Workspace?.Equals(workspace) == true).ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            // After having collected the items, call them
            foreach (var handle in handles)
            {
                handle.ValueAction?.Invoke(value);
                if (extent != null)
                {
                    handle.ExtentAction?.Invoke(extent, value);
                }

                if (extent != null && workspace != null)
                {
                    handle.WorkspaceAction?.Invoke(workspace, extent, value);
                }
            }
        }

        /// <summary>
        /// Has to be called, when the given extent has changed
        /// </summary>
        /// <param name="extent">Extent that was changed</param>
        public void SendChangeEvent(IExtent extent)
        {
            var workspace = extent.GetWorkspace();

            RegisteredEventHandle[] handles;
            try
            {
                _lock.EnterReadLock();
                
                if (_handles.Count == 0)
                    // Nothing to do.
                    return;
                
                handles = _handles.Where(
                    x => x.Extent?.Equals(extent) == true ||
                         x.Workspace?.Equals(workspace) == true).ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            // After having collected the items, call them
            foreach (var handle in handles)
            {
                handle.ExtentAction?.Invoke(extent, null);

                if (workspace != null)
                {
                    handle.WorkspaceAction?.Invoke(workspace, extent, null);
                }
            }
        }

        /// <summary>
        /// Sends a change event that a workspace has been changed
        /// </summary>
        /// <param name="workspace">Changed workspace</param>
        public void SendChangeEvent(IWorkspace workspace)
        {
            RegisteredEventHandle[] handles; 

            try
            {
                _lock.EnterReadLock();
                
                if (_handles.Count == 0) // Nothing to do.
                    return;
                
                handles = _handles.Where(
                    x => x.Workspace?.Equals(workspace) == true).ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            // After having collected the items, call them
            foreach (var handle in handles)
            {
                handle.WorkspaceAction?.Invoke(workspace, null, null);
            }
        }

        public EventHandle RegisterFor(IObject value, Action<IObject> valueAction)
        {
            try
            {
                _lock.EnterWriteLock();

                var eventHandle = new RegisteredEventHandle
                {
                    Value = value,
                    ValueAction = valueAction
                };

                _handles.Add(eventHandle);

                ClassLogger.Trace($"Registered event for: {value} ({_handles.Count})");
                return eventHandle;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public EventHandle RegisterFor(IExtent extent, Action<IExtent, IObject?> extentAction)
        {
            try
            {
                _lock.EnterWriteLock();

                var eventHandle = new RegisteredEventHandle
                {
                    Extent = extent,
                    ExtentAction = extentAction
                };

                _handles.Add(eventHandle);

                ClassLogger.Trace($"Registered event for: {extent} ({_handles.Count})");

                return eventHandle;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public EventHandle RegisterFor(IWorkspace workspace, Action<IWorkspace, IExtent, IObject?> workspaceAction)
        {
            try
            {
                _lock.EnterWriteLock();

                var eventHandle = new RegisteredEventHandle
                {
                    Workspace = workspace,
                    WorkspaceAction = workspaceAction
                };

                _handles.Add(eventHandle);

                ClassLogger.Trace($"Registered event for: {workspace} ({_handles.Count})");

                return eventHandle;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public void Unregister(EventHandle eventHandle)
        {
            try
            {
                _lock.EnterWriteLock();
                _handles.Remove((RegisteredEventHandle) eventHandle);

                ClassLogger.Trace($"Unregistered event ({_handles.Count})");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}