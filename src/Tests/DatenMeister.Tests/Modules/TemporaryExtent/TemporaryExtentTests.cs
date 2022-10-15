using System;
using System.Threading;
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

        [Test]
        public void TestCreationAndDeletion()
        {
            var oldValue = TemporaryExtentLogic.DefaultCleanupTime;
            TemporaryExtentLogic.DefaultCleanupTime = TimeSpan.FromMilliseconds(150);
            
            using var scope = DatenMeisterTests.GetDatenMeisterScope();

            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic);
            var element = temporaryLogic.CreateTemporaryElement(null);
            Assert.That(element, Is.Not.Null);
            
            Thread.Sleep(100);
            
            element.set("name", "Yes");
            
            // Create new temporary logic, like in real life
            temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic);
            temporaryLogic.CleanElements();
            
            // Now find it, this should happen in less than 150 ms
            var found = scope.WorkspaceLogic.Resolve(
                TemporaryExtentPlugin.Uri + "#" + (element as IHasId)?.Id ?? throw new InvalidOperationException("Shall not happen"),
                ResolveType.Default) as IElement;
            Assert.That(found, Is.Not.Null);
            
            // Wait
            Thread.Sleep(100);
            temporaryLogic.CleanElements();
            
            // Check, that it is gone
            found = scope.WorkspaceLogic.Resolve(
                TemporaryExtentPlugin.Uri + "#" + (element as IHasId)?.Id ?? throw new InvalidOperationException("Shall not happen"),
                ResolveType.Default) as IElement;

            Assert.That(found, Is.Null);

            // Restore the clean up time, perhaps some other test is dependent on that
            TemporaryExtentLogic.DefaultCleanupTime = oldValue;
        }
    }
}