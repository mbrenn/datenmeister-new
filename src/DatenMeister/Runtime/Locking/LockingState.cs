using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatenMeister.Runtime.Locking
{
    public class LockingState
    {
        public Task? LockingTask { get; set; }
        
        internal HashSet<string> LockFilePaths { get; } = new HashSet<string>();
        

        /// <summary>
        /// Defines the timespan for the locking
        /// </summary>
        public readonly TimeSpan LockingTimeSpan = TimeSpan.FromSeconds(30);
    }
}