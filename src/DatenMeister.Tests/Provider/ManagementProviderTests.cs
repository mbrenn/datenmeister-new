using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Models;
using DatenMeister.Models.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Provider
{
    [TestFixture]
    public class ManagementProviderTests
    {
        [Test]
        public void TestSettingOfUri()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var uriExtent = scope.WorkspaceLogic.GetManagementWorkspace().FindExtent(WorkspaceNames.UriExtentWorkspaces);

            var userExtent = uriExtent.element("#dm%3A%2F%2F%2F_internal%2Fforms%2Fuser");
            Assert.That(userExtent, Is.Not.Null);
            
            userExtent.set(_ManagementProvider._Extent.uri, "dm:///newusers");

            Assert.That(userExtent.get(_ManagementProvider._Extent.uri), Is.EqualTo("dm:///newusers"));

            var newUsers = scope.WorkspaceLogic.GetManagementWorkspace().extent
                .FirstOrDefault(x => (x as IUriExtent)?.contextURI() == "dm:///newusers");
            
            Assert.That(
                newUsers,
                Is.Not.Null);

            var extentManager = scope.Resolve<ExtentManager>();
            var newUserConfiguration = extentManager.GetLoadConfigurationFor((newUsers as IUriExtent)!);
            Assert.That(newUserConfiguration, Is.Not.Null);
            Assert.That(newUserConfiguration.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._ExtentLoaderConfig.extentUri), Is.EqualTo("dm:///newusers"));
        }
    }
}