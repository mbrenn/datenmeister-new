using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ExportXmiTests
    {
        [Test]
        public async Task TestExportXmi()
        {
            var actionLogic = ActionSetTests.CreateActionLogic();
            ActionSetTests.CreateExtents(actionLogic);

            var temporaryStorage = DatenMeisterTests.GetPathForTemporaryStorage("export.xmi");

            var action = (IElement) InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ExportToXmiAction)
                .SetProperties(new Dictionary<string, object>
                {
                    [_DatenMeister._Actions._ExportToXmiAction.sourcePath] = "dm:///source/",
                    [_DatenMeister._Actions._ExportToXmiAction.filePath] = temporaryStorage,

                });

            await actionLogic.ExecuteAction(action);

            Assert.That(File.Exists(temporaryStorage), Is.True);

            var fileContent = File.ReadAllText(temporaryStorage);
            Assert.That(fileContent.Contains("source1.2"), Is.True);

            File.Delete(temporaryStorage);
        }
    }
}