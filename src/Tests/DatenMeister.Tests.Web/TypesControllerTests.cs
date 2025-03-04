﻿using DatenMeister.WebServer.Controller;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Web
{
    [TestFixture]
    public class TypesControllerTests
    {
        [Test]
        public async Task TestGetTypes()
        {
            using var dm = await DatenMeisterTests.GetDatenMeisterScope();

            var typeController = new TypesController(dm.WorkspaceLogic, dm.ScopeStorage);
            var types = typeController.GetTypes().Value;
            Assert.That(types, Is.Not.Null);
            Assert.That(types.Count, Is.GreaterThan(0));
            /*Assert.That(types.Any(x=>x.name == "DateTime"), Is.True);
            Assert.That(types.Any(x=>x.name == "String"), Is.True);
            Assert.That(types.Any(x=>x.name == "Package"),Is.True);
            Assert.That(types.Any(x=>x.name == "CopyElementAction"), Is.True);*/
        }
    }
}