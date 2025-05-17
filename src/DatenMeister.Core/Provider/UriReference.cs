namespace DatenMeister.Core.Provider
{
    /// <summary>
    /// If the object reference is returned, an indirect reference to the object
    /// is returned. The uri and metaclass Uri is contained within the class.
    /// The uri needs to be resolved by the MOF-Framework
    /// </summary>
    public class UriReference
    {
        /// <summary>
        /// Gets or sets the workspace
        /// </summary>
        public string Workspace { get; set; } = string.Empty;

        /// <summary>
        /// Stores the Uri to which the element is a reference
        /// </summary>
        public string Uri { get; }

        /// <summary>
        /// Initializes a new instance of the UriReference class
        /// </summary>
        /// <param name="uri">Uri to be set as property</param>
        public UriReference(string uri)
        {
            Uri = uri;
        }

        public override bool Equals(object? obj)
        {
            if (obj is UriReference asUriReference)
            {
                return Uri == asUriReference.Uri;
            }

            return false;
        }

        protected bool Equals(UriReference other)
        {
            return Uri == other.Uri;
        }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }

        public override string ToString()
        {
            return "UriReference: " + Uri + " (" + Workspace + ")";
        }
    }
}