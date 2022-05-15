using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.TemporaryExtent;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.TemporaryExtent
{
    [TestFixture]
    public class TemporaryExtentTests
    {
        [Test]
        public void TestCreationAndResolving()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();

            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic);
            var element = temporaryLogic.CreateTemporaryElement(null);
            Assert.That(element, Is.Not.Null);
            
            element.set("name", "Yes");

            // Now find it
            var found = scope.WorkspaceLogic.Resolve(
                TemporaryExtentPlugin.Uri + "#" + (element as IHasId)?.Id ?? throw new InvalidOperationException("Shall not happen"),
                ResolveType.Default) as IElement;

            Assert.That(found, Is.Not.Null);
            Assert.That(found.getOrDefault<string>("name"), Is.EqualTo("Yes"));
        }
    }
}