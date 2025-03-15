using System;
using System.Threading;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.TemporaryExtent;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.TemporaryExtent
{
    [TestFixture]
    public class TemporaryExtentTests
    {
        [Test]
        public async Task TestCreationAndResolving()
        {
            await using var scope = await DatenMeisterTests.GetDatenMeisterScope();

            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic, scope.ScopeStorage);
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
        public async Task TestCreationAndDeletion()
        {
            var oldValue = TemporaryExtentLogic.DefaultCleanupTime;
            TemporaryExtentLogic.DefaultCleanupTime = TimeSpan.FromMilliseconds(150);

            await using var scope = await DatenMeisterTests.GetDatenMeisterScope();

            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic, scope.ScopeStorage);
            var element = temporaryLogic.CreateTemporaryElement(null);
            Assert.That(element, Is.Not.Null);
            
            Thread.Sleep(100);
            
            element.set("name", "Yes");
            
            // Create new temporary logic, like in real life
            temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic, scope.ScopeStorage);
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

        [Test]
        public async Task TestAutomaticRecreationOfTemporaryExtent()
        {
            await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic, scope.ScopeStorage);
            
            Assert.That(temporaryLogic.TryGetTemporaryExtent(), Is.Not.Null);

            var element = temporaryLogic.CreateTemporaryElement(null);
            Assert.That(element, Is.Not.Null);

            scope.WorkspaceLogic.GetDataWorkspace().RemoveExtent(TemporaryExtentLogic.InternalTempUri);
            Assert.That(temporaryLogic.TryGetTemporaryExtent(), Is.Null);
            
            element = temporaryLogic.CreateTemporaryElement(null);
            Assert.That(element, Is.Not.Null);
            Assert.That(temporaryLogic.TryGetTemporaryExtent(), Is.Not.Null);

            scope.WorkspaceLogic.GetDataWorkspace().RemoveExtent(TemporaryExtentLogic.InternalTempUri);
            Assert.That(temporaryLogic.TryGetTemporaryExtent(), Is.Null);
            temporaryLogic.CleanElements();
            Assert.That(temporaryLogic.TryGetTemporaryExtent(), Is.Not.Null);
        }

        [Test]
        public async Task TestFactory()
        {
            await using var scope = await DatenMeisterTests.GetDatenMeisterScope();
            var temporaryLogic = new TemporaryExtentLogic(scope.WorkspaceLogic, scope.ScopeStorage);
            
            var temporaryFactory = new TemporaryExtentFactory(temporaryLogic);
            var element = temporaryFactory.create(null);
            Assert.That(element, Is.Not.Null);
            
            var element2 = temporaryFactory.create(_DatenMeister.TheOne.Forms.__RowForm);
            Assert.That(element2, Is.Not.Null);
            Assert.That(element2.getMetaClass(), Is.Not.Null);
            Assert.That(element2.getMetaClass()!.Equals(_DatenMeister.TheOne.Forms.__RowForm), Is.Not.Null);
        }
    }
}