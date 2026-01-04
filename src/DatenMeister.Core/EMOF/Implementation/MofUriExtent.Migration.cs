namespace DatenMeister.Core.EMOF.Implementation;

public partial class MofUriExtent
{
    public static class Migration
    {
        /// <summary>
        /// Stores a list of migration helpers supporting the move of namespaces and classed
        /// </summary>
        public static List<Func<string, string>> MigrationHelpers { get; set; } = [MigrateDatenMeisterToDm];
        
        /// <summary> 
        /// Performs the migration for the resolving
        /// </summary>
        /// <param name="uri">Uri to be checked</param>
        /// <returns>The migrated uri</returns>
        public static string MigrateUriForResolver(string uri)
        {
            foreach (var migrationHelper in MigrationHelpers)
            {
                uri = migrationHelper(uri);
            }

            return uri;
        }

        /// <summary>
        /// Converts a given URI from the "datenmeister://" format to the "dm://" format
        /// if it matches the predefined internal prefix.
        /// </summary>
        /// <param name="uri">The URI to be processed and potentially migrated.</param>
        /// <returns>The modified URI with the updated prefix or the original URI if no migration is needed.</returns>
        private static string MigrateDatenMeisterToDm(string uri)
        {
            const string prefix = "datenmeister:///_internal";
            if (uri.StartsWith(prefix))
            {
                return "dm:///_internal" + uri.Substring(prefix.Length);
            }

            return uri;
        }
    }
}