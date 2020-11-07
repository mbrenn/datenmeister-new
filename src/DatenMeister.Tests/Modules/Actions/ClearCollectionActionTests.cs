using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ClearCollectionActionTests
    {
        [Test]
        public async Task TestClearCollectionInExtent()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, _) = ActionSetTests.CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ClearCollectionAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._ClearCollectionAction.path] = "dm:///source/",
                });

            Assert.That(source.elements().Count(), Is.GreaterThan(0));
            await actionLogic.ExecuteAction(action);
            Assert.That(source.elements().Count(), Is.EqualTo(0));
        }

        [Test]
        public async Task TestClearCollectionInProperty()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, _) = ActionSetTests.CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ClearCollectionAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._ClearCollectionAction.path] =
                        "dm:///source/?fn=source1&prop=packagedElement",
                });

            Assert.That(source.elements().Count(), Is.GreaterThan(0));
            var source1 = source.elements().First() as IElement;
            Assert.That(source1, Is.Not.Null);
            var packages = source1.getOrDefault<IReflectiveCollection>("packagedElement");
            Assert.That(packages, Is.Not.Null);
            Assert.That(packages.Count(), Is.GreaterThan(0));
            
            await actionLogic.ExecuteAction(action);
            Assert.That(source.elements().Count(), Is.GreaterThan(0));
            Assert.That(packages.Count(), Is.EqualTo(0));

        }
    }
}