﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ItemTransformationTests
    {
        [Test]
        public async Task TestItemTransformation()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            var (source, target) = ActionSetTests.CreateExtents(actionLogic);

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__TransformItemsAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._TransformItemsAction.path] = "dm:///source/",
                    [_DatenMeister._Actions._TransformItemsAction.workspace] = "Data",
                    [_DatenMeister._Actions._TransformItemsAction.runtimeClass] =
                        "DatenMeister.Modules.Actions.Transformations.UpperCaseTransformation"
                });

            var found = source.elements().OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>("name") == "source1");
            Assert.That(found, Is.Not.Null);

            found = source.elements().OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>("name") == "SOURCE1");
            Assert.That(found, Is.Null);

            await actionLogic.ExecuteAction(action);

            found = source.elements().OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>("name") == "SOURCE1");
            Assert.That(found, Is.Not.Null);

            found = source.elements().OfType<IElement>()
                .FirstOrDefault(x => x.getOrDefault<string>("name") == "source1");
            Assert.That(found, Is.Null);
        }
    }
}