using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class UmlDotNetTypeCreatorTests
    {
        [Test]
        public void TestZipCode()
        {
            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                var typeWorkspace = dm.WorkspaceLogic.GetTypesWorkspace();
                var zipCodes = typeWorkspace.Resolve(
                    "datenmeister:///_internal/types/internal#DatenMeister.Modules.ZipExample.ZipCode",
                    ResolveType.Default,
                    false);

                var zipCodesWithState = typeWorkspace.Resolve(
                    "datenmeister:///_internal/types/internal#DatenMeister.Modules.ZipExample.ZipCodeWithState",
                    ResolveType.Default,
                    false);

                Assert.That(zipCodes, Is.Not.Null);
                Assert.That(zipCodesWithState, Is.Not.Null);

                var zipCodegeneralization = ClassifierMethods.GetGeneralizations(zipCodes)?.ToList();
                Assert.That(zipCodegeneralization, Is.Not.Null);
                Assert.That(zipCodegeneralization.Count, Is.EqualTo(0));

                var zipCodesWithStateCodegeneralization = ClassifierMethods.GetGeneralizations(zipCodesWithState)?.ToList();
                Assert.That(zipCodesWithStateCodegeneralization, Is.Not.Null);
                Assert.That(zipCodesWithStateCodegeneralization.Count, Is.EqualTo(1));
                Assert.That(zipCodesWithStateCodegeneralization.First(), Is.EqualTo(zipCodes));

                var zipCodeSpecialization = ClassifierMethods.GetSpecializations(zipCodes)?.ToList();
                Assert.That(zipCodeSpecialization, Is.Not.Null);
                Assert.That(zipCodeSpecialization.Count, Is.EqualTo(2));
                Assert.That(zipCodeSpecialization.Contains(zipCodesWithState), Is.True);
                Assert.That(zipCodeSpecialization.Contains(zipCodes), Is.True);

                var zipCodeWithStateSpecialization = ClassifierMethods.GetSpecializations(zipCodesWithState)?.ToList();
                Assert.That(zipCodeWithStateSpecialization, Is.Not.Null);
                Assert.That(zipCodeWithStateSpecialization.Count, Is.EqualTo(1));
                Assert.That(zipCodeWithStateSpecialization.Contains(zipCodesWithState), Is.True);

                Assert.That(
                    ClassifierMethods.IsSpecializedClassifierOf(
                        zipCodesWithState,
                        zipCodes),
                    Is.True);

                Assert.That(
                    ClassifierMethods.IsSpecializedClassifierOf(
                        zipCodes,
                        zipCodesWithState),
                    Is.False);
            }
        }
    }
}