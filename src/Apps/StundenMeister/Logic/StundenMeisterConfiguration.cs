using System;

namespace StundenMeister.Logic
{
    /// <summary>
    /// Defines the configuration for the StundenMeister
    /// </summary>
    public class StundenMeisterConfiguration
    {
        /// <summary>
        /// Gets or sets the information whether the automatic hibernation detection
        /// shall be active.
        /// If this is active, then there will be no automatic time increase in case
        /// of a hibernation longer than HibernationDetectionTime
        /// </summary>
        public bool HibernationDetectionActive { get; set; } = true;

        /// <summary>
        /// Defines the hibernation time which is accepted for further increases
        /// of the current time recording.
        /// If the last tick has occured for a longer time period than the amount
        /// of time given here, then, there will be no automatic time increase.
        /// </summary>
        public TimeSpan HibernationDetectionTime { get; set; } = TimeSpan.FromHours(0.1);
    }
}