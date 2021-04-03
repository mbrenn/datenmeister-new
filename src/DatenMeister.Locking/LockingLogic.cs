using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Integration;

namespace DatenMeister.Runtime.Locking
{
    public class LockingLogic
    {
        private readonly LockingState _lockingState;

        private readonly bool _isReadOnly;
        
        private static readonly ClassLogger Logger = new ClassLogger(typeof(LockingLogic));

        public LockingLogic(IScopeStorage scopeStorage)
        {
            _lockingState = scopeStorage.Get<LockingState>();
            var integrationSettings = scopeStorage.Get<IntegrationSettings>();
            _isReadOnly = integrationSettings.IsReadOnly;
        }

        private LockingLogic(LockingState lockingState)
        {
            _lockingState = lockingState;
        }

        public static LockingLogic Create(LockingState lockingState)
        {
            return new LockingLogic(lockingState);
        }

        public bool IsLocked(string filePath)
        {
            if (_isReadOnly) return false;
            
            if (File.Exists(filePath))
            {
                string lastUpdateText;
                lock (_lockingState)
                {
                    try
                    {
                        _lockingState.GlobalMutex.WaitOne();
                        lastUpdateText = File.ReadAllText(filePath);
                    }
                    finally
                    {
                        _lockingState.GlobalMutex.ReleaseMutex();
                    }
                }

                if (DateTime.TryParse(
                    lastUpdateText,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var lastUpdate))
                {
                    lock (_lockingState)
                    {
                        if (Math.Abs((DateTime.Now - lastUpdate).TotalSeconds) < _lockingState.LockingTimeSpan.TotalSeconds)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Lock(string filePath)
        {
            if (_isReadOnly) return;
            
            lock (_lockingState)
            {
                if (IsLocked(filePath))
                {
                    throw new IsLockedException($"File {filePath} is already locked", filePath);
                }

                UpdateLockFile(_lockingState, filePath);
                _lockingState.LockFilePaths.Add(filePath);

                _lockingState.LockingTask ??= Task.Run(() => TickLoop(_lockingState));
            }

            Logger.Info("Locking: " + filePath);
        }

        public void Unlock(string filePath)
        {
            if (_isReadOnly) return;
            
            lock (_lockingState)
            {
                _lockingState.LockFilePaths.Remove(filePath);
                
                try
                {
                    _lockingState.GlobalMutex.WaitOne();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                finally
                {
                    _lockingState.GlobalMutex.ReleaseMutex();
                }
            }

            Logger.Info("Unlocking: " + filePath);
        }

        private static void UpdateLockFile(LockingState lockingState, string filePath)
        {
            CreateDirectoryIfNecessary(filePath);

            var dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            lock (lockingState)
            {
                try
                {
                    lockingState.GlobalMutex.WaitOne();
                    File.WriteAllText(filePath, dateTime);
                }
                finally
                {
                    lockingState.GlobalMutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// Performs a tick
        /// </summary>
        /// <param name="lockingState">The locking state</param>
        private static void Tick(LockingState lockingState)
        {
            var n = 0;
            foreach (var filePath in lockingState.LockFilePaths)
            {
                UpdateLockFile(lockingState, filePath);
                n++;
            }
            
            Logger.Info($"Updated {n} Lockfiles");
        }

        /// <summary>
        /// Performs the ticks
        /// </summary>
        private static async void TickLoop(LockingState lockingState)
        {
            while (true)
            {
                TimeSpan lockingTimeSpan;
                lockingTimeSpan = lockingState.LockingTimeSpan;
                await Task.Delay((int) (lockingTimeSpan.TotalMilliseconds * 9 / 10));

                lock (lockingState)
                {
                    if (lockingState.LockingTask == null)
                    {
                        return;
                    }

                    Tick(lockingState);
                }
            }
        }
        
        private static void CreateDirectoryIfNecessary(string filePath)
        {
            // Creates directory if necessary
            var directoryPath = Path.GetDirectoryName(filePath)
                                ?? throw new InvalidOperationException("directoryPath is null");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}