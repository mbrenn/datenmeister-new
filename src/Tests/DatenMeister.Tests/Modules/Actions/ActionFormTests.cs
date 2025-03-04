﻿using DatenMeister.Actions.Forms;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Forms.FormModifications;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Actions
{
    [TestFixture]
    public class ActionFormTests
    {
        [Test]
        public void TestAddingOfActionButtonInActions()
        {
            var form = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Forms.__Form);
            var actionDirect = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__Action);
            var actionIndirect1 = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__ActionSet);
            var actionIndirect2 = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Actions.__DocumentOpenAction);
            var actionNon = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Management.__Extent);

            var formHandler = new ActionFormPlugin.ActionFormModificationPlugin();
            var context = new FormCreationContext()
            {
                DetailElement = actionDirect,
                FormType = _DatenMeister._Forms.___FormType.Row,
                MetaClass = actionDirect.metaclass
            };
            
            // Test 1 
            Assert.That(formHandler.ModifyForm(context, form), Is.True);
            
            // Test 2 
            context.DetailElement = actionIndirect1;
            context.MetaClass = actionIndirect1.metaclass;
            Assert.That(formHandler.ModifyForm(context, form), Is.True);
            
            // Test 3
            context.DetailElement = actionIndirect2;
            context.MetaClass = actionIndirect2.metaclass;
            Assert.That(formHandler.ModifyForm(context, form), Is.True);
            
            // Test 4
            context.DetailElement = actionNon;
            context.MetaClass = actionNon.metaclass;
            Assert.That(formHandler.ModifyForm(context, form), Is.False);
        }
    }
}