using BurnSystems.Logging;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.ChangeEvents;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.Runtime.ChangeEvents;

/// <summary>
/// This class handles all events occuring due to changes, insertion, or deletions of the objects
/// within the extent
/// </summary>
public class ChangeEventManager : IChangeEventManager
{
    private static readonly ClassLogger ClassLogger = new(typeof(ChangeEventManager));

    /// <summary>
    /// Stores the handles that would like to get informed about changes
    /// </summary>
    private readonly List<RegisteredEventHandle> _handles = new();

    /// <summary>
    /// Defines the locking for the change events
    /// </summary>
    private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

    /// <summary>
    /// Sends the change event and waits until it is finished
    /// </summary>
    /// <param name="value">Element which got changed</param>
    public void SendChangeEvent(IObject value)
    {
        SendChangeEventAsync(value).Wait();    
    }
    
    /// <summary>
    /// Sends the change event and waits until it is finished
    /// </summary>
    /// <param name="extent">Extent which got changed</param>
    public void SendChangeEvent(IExtent extent)
    {
        SendChangeEventAsync(extent).Wait();    
    }
    
    /// <summary>
    /// Sends the change event and waits until it is finished
    /// </summary>
    /// <param name="workspace">Workspace which got changed</param>
    public void SendChangeEvent(IWorkspace workspace)
    {
        SendChangeEventAsync(workspace).Wait();    
    }
    
    /// <summary>
    /// Has to be called when the given object has changed
    /// </summary>
    /// <param name="value">Object that was changed</param>
    public async Task SendChangeEventAsync(IObject value)
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
            if (handle.ValueAction != null)
            {
                await handle.ValueAction(value);
            }

            if (extent != null && handle.ExtentAction != null)
            {
                await handle.ExtentAction(extent, value);
            }

            if (extent != null && workspace != null && handle.WorkspaceAction != null)
            {
                await handle.WorkspaceAction(workspace, extent, value);
            }
        }
    }

    /// <summary>
    /// Has to be called, when the given extent has changed
    /// </summary>
    /// <param name="extent">Extent that was changed</param>
    public async Task SendChangeEventAsync(IExtent extent)
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
            if (extent != null && handle.ExtentAction != null)
            {
                await handle.ExtentAction(extent, null);
            }

            if (extent != null && workspace != null && handle.WorkspaceAction != null)
            {
                await handle.WorkspaceAction(workspace, extent, null);
            }
        }
    }

    /// <summary>
    /// Sends a change event that a workspace has been changed
    /// </summary>
    /// <param name="workspace">Changed workspace</param>
    public async Task SendChangeEventAsync(IWorkspace workspace)
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
            if (handle.WorkspaceAction != null)
            {
                await handle.WorkspaceAction(workspace, null, null);
            }
        }
    }

    public EventHandle RegisterFor(IObject value, Func<IObject?, Task> valueAction)
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
    
    
    public EventHandle RegisterFor(IExtent extent, Func<IExtent, IObject?, Task> extentAction)
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

    public EventHandle RegisterFor(IWorkspace workspace, Func<IWorkspace, IExtent?, IObject?, Task> workspaceAction)
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

    /// <summary>
    /// Removes the event from the list of registered events
    /// </summary>
    /// <param name="eventHandle"></param>
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