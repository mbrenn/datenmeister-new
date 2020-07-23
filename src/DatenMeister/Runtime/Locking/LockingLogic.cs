using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BurnSystems.Logging;
using DatenMeister.Integration;

namespace DatenMeister.Runtime.Locking
{
    public class LockingLogic
    {
        private readonly LockingState _lockingState;
        private static readonly ClassLogger Logger = new ClassLogger(typeof(LockingLogic));

        public LockingLogic(IScopeStorage scopeStorage)
        {
            _lockingState = scopeStorage.Get<LockingState>();
        }

        private LockingLogic(LockingState lockingState)
        {
            _lockingState = lockingState;
        }

        public LockingLogic Create(LockingState lockingState)
        {
            return new LockingLogic(lockingState);
        }

        public bool IsLocked(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lastUpdateText = File.ReadAllText(filePath);
                if (DateTime.TryParse(lastUpdateText, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var lastUpdate))
                {
                    lock (_lockingState)
                    {
                        if (Math.Abs((DateTime.Now - lastUpdate).TotalSeconds) < _lockingState.LockingTimeSpan.TotalSeconds)
                        {
                            return true;
                        }
                    }
                };
            }

            return false;
        }

        public void Lock(string filePath)
        {
            lock (_lockingState)
            {
                _lockingState.LockFilePaths.Add(filePath);

                if (_lockingState.LockingTask == null)
                {
                    _lockingState.LockingTask = Task.Run(TickLoop);
                }
            }

            UpdateLockFile(filePath);
            
            Logger.Info("Locking: " + filePath);
        }

        private void UpdateLockFile(string filePath)
        {
            CreateDirectoryIfNecessary(filePath);

            var dateTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            File.WriteAllText(filePath, dateTime);
            Logger.Info("Updated Lockfile: " + filePath);
        }

        public void Unlock(string filePath)
        {
            lock (_lockingState)
            {
                _lockingState.LockFilePaths.Remove(filePath);
            }
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            Logger.Info("Unlocking: " + filePath);
        }

        public void Tick()
        {
            lock (_lockingState)
            {
                foreach (var filePath in _lockingState.LockFilePaths)
                {
                    UpdateLockFile(filePath);
                }
            }
        }

        /// <summary>
        /// Performs the ticks
        /// </summary>
        public async void TickLoop()
        {
            while (true)
            {
                TimeSpan lockingTimeSpan;

                lock (_lockingState)
                {
                    lockingTimeSpan = _lockingState.LockingTimeSpan;
                    if (_lockingState.LockingTask == null)
                    {
                        return;
                    }
                }

                Tick();
                await Task.Delay((int) (lockingTimeSpan.TotalMilliseconds * 9 / 10));
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