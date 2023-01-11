namespace DatenMeister.WebServer.Library.Helper
{
    public static class MvcUrlEncoder
    {
        /// <summary>
        /// Decodes the path for the paths which were created by the ASP.Net Core MVC Framework
        /// </summary>
        /// <param name="pathToBeDecoded">The path that shall be decoded</param>
        /// <returns>The returned value</returns>
        public static string? DecodePath(string? pathToBeDecoded)
        {
            return pathToBeDecoded?
                .Replace("%2F", "/")
                .Replace("%2f", "/")
                .Replace("%25", "#");;
        }

        /// <summary>
        /// Decodes the path for the paths which were created by the ASP.Net Core MVC Framework
        /// </summary>
        /// <param name="pathToBeDecoded">The path that shall be decoded</param>
        /// <returns>The returned value or an empty string, pathToBeDecoded was null</returns>
        public static string DecodePathOrEmpty(string? pathToBeDecoded)
            => DecodePath(pathToBeDecoded) ?? string.Empty;
        
        /// <summary>
        /// Encodes the path for the paths which were created by the ASP.Net Core MVC Framework
        /// </summary>
        /// <param name="pathToBeEncoded">The path that shall be Encoded</param>
        /// <returns>The returned value</returns>
        public static string EncodePath(string pathToBeEncoded)
        {
            return pathToBeEncoded
                .Replace( "/", "%2F")
                .Replace("#", "%25");
        }
    }
}