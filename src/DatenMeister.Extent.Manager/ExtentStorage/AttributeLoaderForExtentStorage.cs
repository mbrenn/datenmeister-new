namespace DatenMeister.Extent.Manager.ExtentStorage
{
    /*
    public static class AttributeLoaderForExtentStorage
    {
        private static readonly ClassLogger Logger = new ClassLogger(typeof(AttributeLoaderForExtentStorage));

        /// <summary>
        /// Goes through all loaded types and add the extentstorage mapping as configured by the ConfiguredBy attributes
        /// </summary>
        /// <param name="map"></param>
        public static void LoadAllExtentStorageConfigurationsFromAssembly(this ConfigurationToExtentStorageMapper map)
        {
            foreach (var type in
                from pair
                    in PluginManager.GetTypesOfAssemblies<ConfiguredByAttribute>()
                let type = pair.Key
                where !map.ContainsConfigurationFor(pair.Value.ConfigurationType)
                select type)
            {
                map.MapExtentLoaderType(type);
            }
        }

        public static void MapExtentLoaderType(this ConfigurationToExtentStorageMapper map, Type type)
        {
            foreach (
                var customAttribute in type.GetTypeInfo().GetCustomAttributes(typeof(ConfiguredByAttribute), false))
            {
                if (customAttribute is ConfiguredByAttribute configuredByAttribute)
                {
                    map.AddMapping(configuredByAttribute.ConfigurationType,
                        scope =>
                        {
                            if (scope == null) throw new ArgumentException(nameof(scope));
                            if (!(Activator.CreateInstance(type) is IProviderLoader result))
                            {
                                throw new InvalidOperationException(
                                    $"Activated Type is not of Type IProviderLoader: {type.FullName}");
                            }
                            
                            result.WorkspaceLogic = scope.WorkspaceLogic;
                            result.ScopeStorage = scope.ScopeStorage;
                            return result;
                        });

                    Logger.Trace(
                        $"Extent loader '{configuredByAttribute.ConfigurationType.Name}' configures '{type.Name}'");
                }
            }
        }
    }*/
}