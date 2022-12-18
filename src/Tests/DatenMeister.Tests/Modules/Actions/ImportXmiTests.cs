using System.Linq;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ImportXmiTests
    {
        [Test]
        public void TestOfExtent()
        {    
            var (workspaceLogic, scopeStorage) = DatenMeisterTests.GetDmInfrastructure();

            var newExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test", scopeStorage);
            workspaceLogic.AddExtent(workspaceLogic.GetDataWorkspace(), newExtent);

            var xmi = @"<xmi>
    <meta p2:id=""1ca6dd3b-be32-4cd4-896b-23506a23a81d"" __uri=""dm:///export"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" xmlns=""http://datenmeister.net/"" />
    <item p2:type=""dm:///_internal/types/internal#IssueMeister.Issue"" p2:id=""2"" description=""Link to item does link to 404 and not to Item1"" state=""Closed"" name=""Detail Form - Bread Crumb1"" id=""2"" _toBeCleanedUp=""09/18/2022 12:22:44"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" />
    <item p2:type=""dm:///_internal/types/internal#IssueMeister.Issue"" p2:id=""1"" _toBeCleanedUp=""09/18/2022 12:22:02"" id=""1"" name=""Moving up and Down"" state=""Closed"" description=""List Tables shall support the move up and move down of items&#xA;&#xA;fdsa"" xmlns:p2=""http://www.omg.org/spec/XMI/20131001"" />
</xmi>";    

            // Performs the import via the action handler...
            var actionLogic = new ActionLogic(workspaceLogic, scopeStorage);
            var importXmi = new ImportXmiActionHandler();
            var action = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ImportXmiAction);
            action.set(_DatenMeister._Actions._ImportXmiAction.workspace, WorkspaceNames.WorkspaceData);
            action.set(_DatenMeister._Actions._ImportXmiAction.itemUri, "dm:///test");
            action.set(_DatenMeister._Actions._ImportXmiAction.xmi, xmi);
            
            importXmi.Evaluate(actionLogic, action);

            Assert.That(newExtent.elements().Count(), Is.EqualTo(2));
            Assert.That(newExtent.elements().OfType<IElement>().First().getOrDefault<string>("state"),
                Is.EqualTo("Closed"));
            Assert.That(newExtent.elements().OfType<IElement>().ElementAt(1).getOrDefault<string>("name"),
                Is.EqualTo("Moving up and Down"));
        }
    }
}