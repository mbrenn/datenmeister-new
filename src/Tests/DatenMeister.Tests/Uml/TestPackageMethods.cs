﻿using System.Linq;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestPackageMethods
    {
        [Test]
        public void TestImportOfPackageIntoExtent()
        {
            using var scope = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();

            var extent = new MofUriExtent(new InMemoryProvider(), "dm:///test", null);

            PackageMethods.ImportByManifest(
                typeof(TestPackageMethods),
                "DatenMeister.Tests.Xmi.PackageTest.xmi",
                "Internal",
                extent,
                string.Empty);

            Assert.That(extent.elements().Count(), Is.EqualTo(2));
            var firstElement = extent.elements().ElementAt(0);
            var secondElement = extent.elements().ElementAt(1);

            Assert.That(NamedElementMethods.GetName(firstElement), Is.EqualTo("Default"));
            Assert.That(NamedElementMethods.GetName(secondElement), Is.EqualTo("IssueMeister"));
        }
    }
}