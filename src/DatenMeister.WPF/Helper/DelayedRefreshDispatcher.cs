using System;
using System.Threading;
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
        private static readonly ClassLogger ClassLogger = new ClassLogger(typeof(DelayedRefreshDispatcher));

        /// <summary>
        /// The dispatcher being used
        /// </summary>
        private readonly Dispatcher _dispatcher;

        /// <summary>
        /// The action being called
        /// </summary>
        private readonly Action _action;

        private static int _instanceCount;

        /// <summary>
        /// Stores a flag whether a refresh already has occured.
        /// If this value is false, then a refresh is outstanding 
        /// </summary>
        private bool _isRefreshed = true;

        private int _instance;

        /// <summary>
        /// Initializes a new instance of the DelayedRefreshDispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher being used</param>
        /// <param name="action">Action being called</param>
        public DelayedRefreshDispatcher(Dispatcher dispatcher, Action action)
        {
            _dispatcher = dispatcher;
            _action = action;
            _instance = Interlocked.Add(ref _instanceCount, 1);
        }

        /// <summary>
        /// Defines the minimum dispatch time for which th dispatching will be delayed before the dispatched function will be called
        /// </summary>
        public TimeSpan MinDispatchTime { get; set; } = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Defines the maximum delayted time for which the dispatching will be delayed if the refresh is requested continuously.
        /// </summary>
        public TimeSpan MaxDispatchTime { get; set; } = TimeSpan.FromMilliseconds(1000);

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
                _isRefreshed = false;
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

                // ClassLogger.Trace($"#{_instance}: Dispatch in {MinDispatchTime.TotalMilliseconds} ms");
                Task.Delay(MinDispatchTime).ContinueWith(t =>
                {
                    // ClassLogger.Trace($"#{_instance}: Got Called");
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
                        // ClassLogger.Trace($"#{_instance}: Retry Dispatch in {dispatchTime.TotalMilliseconds} ms");

                        Task.Delay(dispatchTime).ContinueWith(t => CheckForRefresh());
                        return;
                    }
                }

                if (_isRefreshed)
                {
                    // We already got a refresh, so everything is fine
                    return;
                }

                // ClassLogger.Trace($"#{_instance}: Dispatched after {delta.TotalMilliseconds} ms");

                _lastRefreshTime = DateTime.MinValue;
                _refreshTimeStamp = DateTime.MinValue;
                _timerRunning = false;
                _isRefreshed = true;
            }

            _dispatcher.Invoke(() => _action());
        }

        /// <summary>
        /// Forces a complete refresh
        /// </summary>
        public void ForceRefresh()
        {
            lock (_syncObject)
            {
                _isRefreshed = true;
            }

            _dispatcher.Invoke(() => _action());
        }
    }
}