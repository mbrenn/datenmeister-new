namespace DatenMeister.Core.EMOF.Implementation
{
    public partial class MofUriExtent
    {
        public class Migration
        {
            /// <summary>
            /// Performs the migration for the resolving
            /// </summary>
            /// <param name="uri">Uri to be checked</param>
            /// <returns>The migrated uri</returns>
            public static string MigrateUriForResolver(string uri)
            {
                var prefix = "datenmeister:///";
                if (uri.StartsWith(prefix))
                {
                    return "dm:///" + uri.Substring(prefix.Length);
                }

                return uri;
            }
        }
    }
}