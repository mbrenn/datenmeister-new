using DatenMeister.Extent.Verifier;
using NUnit.Framework;

namespace DatenMeister.Tests.Verifiers
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task TestThatDatenMeisterIsWithoutVerificationIssues()
        {
            var dm = await DatenMeisterTests.GetDatenMeisterScope();
            var verifier = new Verifier(dm.WorkspaceLogic, dm.ScopeStorage);
            Initializer.InitWithDefaultVerifiers(dm.WorkspaceLogic, verifier);

            await verifier.VerifyExtents();

            foreach (var verifierEntry in verifier.VerifyEntries)
            {
                Console.WriteLine(verifierEntry.ToString());
            }

            Assert.That(verifier.VerifyEntries.Count, Is.EqualTo(0));
        }
    }
}