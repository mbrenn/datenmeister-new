using System.Collections.Generic;
using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [Test]
        public void ExtentTest()
        {
            var kernel = new ContainerBuilder();
            var builder = kernel.UseDatenMeister(new IntegrationSettings());
            using (var scope = builder.BeginLifetimeScope())
            {
                var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
                var workspaceExtent = workspaceLogic.FindExtent(ExtentOfWorkspaces.WorkspaceUri);
                Assert.That(workspaceExtent, Is.Not.Null);
                var asData = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameData);
                var asManagement = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameManagement);
                var asTypes = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameTypes);
                var asMof = workspaceExtent.elements().Cast<IElement>().First(x => x.get("id").ToString() == WorkspaceNames.NameMof);

                Assert.That(asData, Is.Not.Null);
                Assert.That(asManagement, Is.Not.Null);
                Assert.That(asTypes, Is.Not.Null);
                Assert.That(asMof, Is.Not.Null);

                // Get the extents
                var extents = (asMof.get("extents") as IEnumerable<object>)?.ToList();
                Assert.That(extents, Is.Not.Null);

                var mofExtent = extents.Cast<IElement>().First( x=> x.get("uri").ToString() == WorkspaceNames.UriMof);
                Assert.That(mofExtent, Is.Not.Null);
            }
        }
    }
}