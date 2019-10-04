using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using BurnSystems.Logging;

namespace DatenMeister.WPF.Helper
{
    /// <summary>
    /// This class supports the delayed refreshing of forms to prevent unnecessary refreshes of the window content
    /// The refresh is delayed minimally by the min dispatch time. If the update is continuously called, the maximum
    /// delay time defines delay until which the refresh function is called latest.
    /// </summary>
    public class DelayedRefreshDispatcher
    {
        private readonly ClassLogger ClassLogger = new ClassLogger(typeof(DelayedRefreshDispatcher));

        /// <summary>
        /// The dispatcher being used
        /// </summary>
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// The action being called
        /// </summary>
        private readonly Action _action;

        /// <summary>
        /// Initializes a new instance of the DelayedRefreshDispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher being used</param>
        /// <param name="action">Action being called</param>
        public DelayedRefreshDispatcher(Dispatcher dispatcher, Action action)
        {
            _dispatcher = dispatcher;
            _action = action;
        }

        /// <summary>
        /// Defines the minimum dispatch time for which th dispatching will be delayed before the dispatched function will be called
        /// </summary>
        public TimeSpan MinDispatchTime { get; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Defines the maximum delayted time for which the dispatching will be delayed if the refresh is requested continuously.
        /// </summary>
        public TimeSpan MaxDispatchTime { get; } = TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Defines the timestamp in which the last refresh was asked for
        /// </summary>
        private DateTime _refreshTimeStamp = DateTime.MinValue;

        /// <summary>
        /// Defines the time in which the refresh occured the last time
        /// </summary>
        private DateTime _lastRefreshTime = DateTime.MinValue;

        /// <summary>
        /// Gets the flag whether a timer is running
        /// </summary>
        private bool _timerRunning;

        /// <summary>
        /// Defines the synchronization object
        /// </summary>
        private readonly object _syncObject = new object();

        /// <summary>
        /// This method shall be called if a request is required
        /// </summary>
        public void RequestRefresh()
        {
            lock (_syncObject)
            {
                if (_timerRunning)
                {
                    return;
                }

                _timerRunning = true;
                _refreshTimeStamp = DateTime.Now;
                if (_lastRefreshTime == DateTime.MinValue)
                {
                    _lastRefreshTime = DateTime.Now;
                }

                ClassLogger.Trace($"Dispatch in {MinDispatchTime.TotalMilliseconds} ms");
                Task.Delay(MinDispatchTime).ContinueWith(t =>
                {
                    ClassLogger.Trace("Got Called");
                    CheckForRefresh();
                });
            }
        }

        /// <summary>
        /// Checks whether a refresh is necessary
        /// </summary>
        private void CheckForRefresh()
        {
            lock (_syncObject)
            {
                var delta = DateTime.Now - _refreshTimeStamp;

                if (delta < MinDispatchTime)
                {
                    // Dispatch time has not run
                    delta = DateTime.Now - _lastRefreshTime;
                    if (delta < MaxDispatchTime)
                    {
                        var dispatchTime = MinDispatchTime - delta;
                        // But maximum delay has not occured
                        ClassLogger.Trace($"Retry Dispatch in {dispatchTime.TotalMilliseconds} ms");

                        Task.Delay(dispatchTime).ContinueWith(t => CheckForRefresh());
                        return;
                    }
                }

                ClassLogger.Trace($"Dispatched after {delta.TotalMilliseconds} ms");

                _lastRefreshTime = DateTime.MinValue;
                _refreshTimeStamp = DateTime.MinValue;
                _timerRunning = false;
            }

            _dispatcher.Invoke(() => _action());
        }
    }
}