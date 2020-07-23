using System;

namespace DatenMeister.Runtime.Locking
{
    public class IsLockedException : InvalidOperationException
    {
        public IsLockedException(string filePath)
        {
            FilePath = filePath;
        }

        public IsLockedException(string message, string filePath) : base(message)
        {
            FilePath = filePath;
        }

        public IsLockedException(string message, Exception innerException, string filePath) : base(message,
            innerException)
        {
            FilePath = filePath;
        }

        /// <summary>
        /// Gets or sets the file path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"File is locked: {FilePath}";
        }
    }
}