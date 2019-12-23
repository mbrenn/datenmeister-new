﻿using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class ViewLogicTests
    {
        [Test]
        public void TestAvailabiltyOfInternalViews()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<FormLogic>();
            var internalViewExtent = viewLogic.GetInternalFormExtent();

            Assert.That(internalViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestAvailabiltyOfUserViews()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<FormLogic>();
            var userViewExtent = viewLogic.GetUserFormExtent();

            Assert.That(userViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestGetAllViews()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<FormLogic>();
            var viewExtent = viewLogic.GetUserFormExtent();
            var factory = new MofFactory(viewExtent);
            
            var listForm = viewExtent.FindInMeta<_FormAndFields>(x => x.__ListForm);
            
            
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

        /*
         * Test is obsolete since the view associations are currently not stored in the xmi
        [Test]
        public void TestGetAllViewAssociations()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<ViewLogic>();
            var n = 0;

            foreach (var _ in viewLogic.GetAllViewAssociations())
            {
                n++;
            }

            Assert.That(n, Is.GreaterThan(0));
        }*/
    }
}