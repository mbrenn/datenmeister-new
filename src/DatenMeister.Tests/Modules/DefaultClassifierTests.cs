using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Modules.DefaultTypes;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class DefaultClassifierTests
    {
        [Test]
        public void TestDefaultPackage()
        {
            var workspaceData = WorkspaceLogic.InitDefault();
            var dataExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var typeExtent = new MofUriExtent(new InMemoryProvider(), "dm:///types");
            var umlExtent = new MofUriExtent(new InMemoryProvider(), "dm:///uml");

            workspaceData.Uml.AddExtent(umlExtent);
            workspaceData.Types.AddExtent(typeExtent);
            workspaceData.Data.AddExtent(dataExtent);

            var defaultClassifier = new DefaultClassifierHints();
            var dataPackage = defaultClassifier.GetDefaultPackageClassifier(dataExtent);
            var typePackage = defaultClassifier.GetDefaultPackageClassifier(typeExtent);
            var umlPackage = defaultClassifier.GetDefaultPackageClassifier(umlExtent);

            Assert.That(dataPackage?.@equals(_DatenMeister.TheOne.CommonTypes.Default.__Package) == true);
            Assert.That(typePackage?.@equals(_UML.TheOne.Packages.__Package) == true);
            Assert.That(umlPackage?.@equals(_UML.TheOne.Packages.__Package) == true);
        }
    }
}
