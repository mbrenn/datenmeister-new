using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using ICSharpCode.SharpZipLib.Core;

namespace StundenMeister.Model
{
    public class TimeRecording
    {
        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public bool isActive { get; set; }

        public CostCenter costCenter { get; set; }
        
        public int timeSpanSeconds { get; }
        
        public int timeSpanHours { get; }
        
        /// <summary>
        /// Gets the timespan between the start date and the end date in seconds 
        /// </summary>
        /// <param name="timeRecording">Time recording to be evaluated</param>
        /// <returns>Number of seconds</returns>
        public static int GetTimeSpanSeconds(IObject timeRecording)
        {
            var start = timeRecording.getOrDefault<DateTime>(nameof(startDate));
            var end = timeRecording.getOrDefault<DateTime>(nameof(endDate));
            
            return (int) Math.Ceiling((end - start).TotalSeconds);
        }
        
        /// <summary>
        /// Gets the timespan between the start date and the end date in seconds 
        /// </summary>
        /// <param name="timeRecording">Time recording to be evaluated</param>
        /// <returns>Number of seconds</returns>
        public static double GetTimeSpanHours(IObject timeRecording)
        {
            var start = timeRecording.getOrDefault<DateTime>(nameof(startDate));
            var end = timeRecording.getOrDefault<DateTime>(nameof(endDate));
            
            return (end - start).TotalHours;
        }
    }
}