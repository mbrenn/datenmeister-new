﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DatenMeister.Locking
{
    public class LockingState
    {
        public Task? LockingTask { get; set; }
        
        internal HashSet<string> LockFilePaths { get; } = new();

        internal Mutex GlobalMutex { get; } = new(false, "Global\\DatenMeister.LockingState");
        
        /// <summary>
        /// Defines the timespan for the locking
        /// </summary>
        public TimeSpan LockingTimeSpan { get; set; } = TimeSpan.FromSeconds(30);
    }
}