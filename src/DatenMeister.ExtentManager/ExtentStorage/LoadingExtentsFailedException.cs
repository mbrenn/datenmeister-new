using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DatenMeister.ExtentManager.ExtentStorage
{
    public class LoadingExtentsFailedException : Exception
    {
        /// <summary>
        /// Gets the failed extents that could not be loaded
        /// </summary>
        public List<string> FailedExtents { get; } = new List<string>();

        public LoadingExtentsFailedException(List<string> failedExtents)
        {
            FailedExtents = failedExtents;
        }

        public LoadingExtentsFailedException(string message, List<string> failedExtents) : base(message)
        {
            FailedExtents = failedExtents;
        }

        public LoadingExtentsFailedException(string message, Exception innerException, List<string> failedExtents) : base(message, innerException)
        {
            FailedExtents = failedExtents;
        }

        protected LoadingExtentsFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}