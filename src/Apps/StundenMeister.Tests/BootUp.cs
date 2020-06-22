using System;
using System.IO;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Integration;
using DatenMeister.Runtime;
using NUnit.Framework;
using StundenMeister.Logic;
using StundenMeister.Model;

namespace StundenMeister.Tests
{
    public class BootUp
    {
        [Test]
        public void TestBootUp()
        {
            using (var dm = CreateDatenMeisterEnvironment())
            {
                var logic = dm.Resolve<StundenMeisterPlugin>();
                Assert.That(logic, Is.Not.Null);

                Assert.That(StundenMeisterData.TheOne, Is.Not.Null);
                Assert.That(StundenMeisterData.TheOne.ClassCostCenter, Is.Not.Null);
                Assert.That(StundenMeisterData.TheOne.ClassCostCenter, Is.Not.Null);
                Assert.That(
                    StundenMeisterData.TheOne.ClassCostCenter.getOrDefault<string>(
                        _UML._CommonStructure._NamedElement.name),
                    Is.EqualTo(nameof(CostCenter)));

                Assert.That(StundenMeisterData.TheOne.Extent, Is.Not.Null);
            }
        }

        public static IDatenMeisterScope CreateDatenMeisterEnvironment()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "StundenMeister.Tests");
            
            var settings = new IntegrationSettings
            {
                DatabasePath = path
            };
            
            GiveMe.DropDatenMeisterStorage(settings);

            var datenMeister = GiveMe.DatenMeister(settings);
            var logic = datenMeister.Resolve<StundenMeisterPlugin>();
            logic.Data.Extent.elements().clear();
            return datenMeister;
        }
    }
}