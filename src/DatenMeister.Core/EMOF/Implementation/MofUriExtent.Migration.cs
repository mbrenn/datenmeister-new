namespace DatenMeister.Core.EMOF.Implementation;

public partial class MofUriExtent
{
    public static class Migration
    {
        /// <summary>
        /// Stores a list of migration helpers supporting the move of namespaces and classed
        /// </summary>
        public static List<Func<string, string?>> MigrationHelpers { get; set; } = [MigrateDatenMeisterToDm, IgnoreMofTags];
        
        /// <summary> 
        /// Performs the migration for the resolving
        /// </summary>
        /// <param name="uri">Uri to be checked</param>
        /// <returns>The migrated uri</returns>
        public static string? MigrateUriForResolver(string? uri)
        {
            if(uri == null) return null;
            
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

        /// <summary>
        /// Ignores specific URIs matching predefined MOF tag addresses by converting them to null.
        /// </summary>
        /// <param name="uri">The URI to be checked and potentially ignored.</param>
        /// <returns>The original URI if it does not match the ignored MOF tag, otherwise null.</returns>
        private static string? IgnoreMofTags(string uri)
        {
            const string ignore = "http://www.omg.org/spec/MOF/20131001#Tag";
            if(uri == ignore) return null;

            return uri;
        }
    
        /// <summary>
        /// Adds a converter which converts the first parameter to the second parameter fitting to that function
        /// </summary>
        /// <param name="helpers">The list of migration helpers</param>
        /// <param name="from">The uri to be converted</param>
        /// <param name="to">The uri to be converted to</param>
        public static void AddConverter(string from, string to)
        {
            MigrationHelpers.Add(uri => uri == from ? to : uri);
        }
    }
}