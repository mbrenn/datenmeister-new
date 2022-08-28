using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class MoveOrCopyActionTests
    {
        [Test]
        public async Task TestCopyAction()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var elementSource = source.element("#source1.1");
            var elementTargetNonExisting = target.element("#source1.1");
            var elementTarget = target.element("#target1.1");

            Assert.That(elementSource, Is.Not.Null);
            Assert.That(elementTarget, Is.Not.Null);
            Assert.That(elementTargetNonExisting, Is.Null);

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__MoveOrCopyAction);
            action.set(_DatenMeister._Actions._MoveOrCopyAction.source, elementSource);
            action.set(_DatenMeister._Actions._MoveOrCopyAction.target, elementTarget);
            action.set(
                _DatenMeister._Actions._MoveOrCopyAction.actionType,
                _DatenMeister._Actions.___MoveOrCopyType.Copy);

            await actionLogic.ExecuteAction(action);

            var elementInSourceAfter = source.element("#source1.1");
            var elementInTargetAfter = target.elements().GetAllDescendants()
                .WhenPropertyHasValue("name", "source1.1")
                .FirstOrDefault();
            Assert.That(elementInSourceAfter, Is.Not.Null);
            Assert.That(elementInTargetAfter, Is.Not.Null);

            var inTargetCollection = elementTarget.getOrDefault<IReflectiveCollection>(
                    DefaultClassifierHints.GetDefaultPackagePropertyName(elementTarget!))
                .OfType<IElement>()
                .ToList();
            Assert.That(inTargetCollection.Count, Is.EqualTo(1));
            Assert.That(
                inTargetCollection.Any(x =>
                    x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) == "source1.1"),
                Is.True);
        }
        
        [Test]
        public async Task TestMoveAction()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var elementSource = source.element("#source1.1");
            var elementTarget = target.element("#target1.1");

            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__MoveOrCopyAction);
            action.set(_DatenMeister._Actions._MoveOrCopyAction.source, elementSource);
            action.set(_DatenMeister._Actions._MoveOrCopyAction.target, elementTarget);
            action.set(
                _DatenMeister._Actions._MoveOrCopyAction.actionType,
                _DatenMeister._Actions.___MoveOrCopyType.Move);

            await actionLogic.ExecuteAction(action);

            var elementInSourceAfter = source.element("#source1.1");
            var elementInTargetAfter = target.elements()
                .GetAllDescendants()
                .WhenPropertyHasValue("name", "source1.1")
                .FirstOrDefault();
            Assert.That(elementInSourceAfter, Is.Null);
            Assert.That(elementInTargetAfter, Is.Not.Null);

            var inTargetCollection = elementTarget.getOrDefault<IReflectiveCollection>(
                    DefaultClassifierHints.GetDefaultPackagePropertyName(elementTarget!))
                .OfType<IElement>()
                .ToList();
            
            Assert.That(inTargetCollection.Count, Is.EqualTo(1));
            Assert.That(
                inTargetCollection.Any(x =>
                    x.getOrDefault<string>(_UML._CommonStructure._NamedElement.name) == "source1.1"),
                Is.True);
        }
    }
}