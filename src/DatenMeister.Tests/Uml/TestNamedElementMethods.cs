﻿using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.EMOF;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestNamedElementMethods
    {
        [Test]
        public void TestFullName()
        {
            using var builder = DatenMeisterTests.GetDatenMeisterScope();
            using var scope = builder.BeginLifetimeScope();
            var workspaceCollection = scope.Resolve<IWorkspaceLogic>();
            var workspaceLogic = scope.Resolve<WorkspaceLogic>();

            // Gets the logic
            var feature =
                workspaceLogic.GetUmlWorkspace().Resolve(_UML.TheOne.Classification.__Feature.GetUri() ?? "Uri",
                    ResolveType.Default) as IElement;
            Assert.That(feature, Is.Not.Null);
                        
            var fullName = NamedElementMethods.GetFullName(feature);

            Assert.That(fullName, Is.Not.Null);
            Assert.That(fullName, Is.EqualTo("UML::Classification::Feature"));

            var umlExtent = workspaceCollection.GetWorkspace(WorkspaceNames.WorkspaceUml).FindExtent(WorkspaceNames.UriExtentUml);
            // now the other way
            var foundElement = NamedElementMethods.GetByFullName(umlExtent.elements(), fullName);
            Assert.That(foundElement, Is.EqualTo(feature));
        }
        
    }
}