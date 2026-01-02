using System.Reflection;
using Autofac;
using DatenMeister.Integration.DotNet;
using DatenMeister.Plugins;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime;

[TestFixture]
public class PluginTests
{
    [Test]
    public async Task TestAvailabilityOfLoadedPlugins()
    {
        var kernel = new ContainerBuilder();
        var dmScope = await kernel.UseDatenMeister(DatenMeisterTests.GetIntegrationSettings());

        var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                            ?? throw new InvalidOperationException("directoryName is not set");

        // We will start the plugins
        var pluginManager = dmScope.Resolve<PluginManager>();
        var pluginLoader = new MiniPluginLoader();
        pluginLoader.LoadAssembliesFromFolder(directoryName);
        Assert.That(
            pluginLoader.GetPluginTypes().Count, Is.GreaterThan(0),
            "No plugin was found by MiniLoader, something obscure happened");

        await pluginManager.StartPlugins(dmScope, new MiniPluginLoader(), PluginLoadingPosition.BeforeBootstrapping);

        // We have now started the plugins and we check the existence and availability of the PluginState
        Assert.That(pluginManager, Is.Not.Null);

        // Check that there is at least one instantiiated Pluing
        Assert.That(pluginManager.InstantiatedPlugins.Count(), Is.GreaterThan(0));
    }

    public class MiniPluginLoader: IPluginLoader
    {
        /// <summary>
        /// Stores the default loaded which is used to figure out potential assemblies.
        /// </summary>
        private readonly IPluginLoader _defaultLoader = new DefaultPluginLoader(); 
        
        public void LoadAssembliesFromFolder(string path)
        {
            _defaultLoader.LoadAssembliesFromFolder(path);
        }

        public List<Type> GetPluginTypes()
        {
            return _defaultLoader.GetPluginTypes()
                .Where(x => x.FullName?.Contains("TypeIndexPlugin") == true)
                .ToList();
        }
    }
}