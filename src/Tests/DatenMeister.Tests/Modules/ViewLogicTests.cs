﻿using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Forms;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class ViewLogicTests
    {
        [Test]
        public void TestAvailabiltyOfInternalViews()
        {
            var datenMeister = DatenMeisterTests.GetDatenMeisterScope();
            var viewLogic = datenMeister.Resolve<FormMethods>();
            var internalViewExtent = viewLogic.GetInternalFormExtent();

            Assert.That(internalViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestAvailabilityOfUserViews()
        {
            var datenMeister = DatenMeisterTests.GetDatenMeisterScope();
            var viewLogic = datenMeister.Resolve<FormMethods>();
            var userViewExtent = viewLogic.GetUserFormExtent();

            Assert.That(userViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestGetAllViews()
        {
            var datenMeister = DatenMeisterTests.GetDatenMeisterScope();
            var viewLogic = datenMeister.Resolve<FormMethods>();
            var viewExtent = viewLogic.GetUserFormExtent();
            var factory = new MofFactory(viewExtent);
            
            var listForm = _DatenMeister.TheOne.Forms.__TableForm;
            
            var n = 0;
            foreach (var _ in viewLogic.GetAllForms())
            {
                n++;
            }

            var oldAmount = n;

            // Adds a new element and checks, if the new element is found via GetAllForms
            var createdElement = factory.create(listForm);
            viewExtent.elements().add(createdElement);
            
            n = 0;
            foreach (var _ in viewLogic.GetAllForms())
            {
                n++;
            }
            var newAmount = n;

            Assert.That(newAmount - oldAmount, Is.EqualTo(1));
        }
    }
}