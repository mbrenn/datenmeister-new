using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Models;
using DatenMeister.Forms.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules;

[TestFixture]
public class ViewLogicTests
{
    [Test]
    public async Task TestAvailabiltyOfInternalViews()
    {
        var datenMeister = await DatenMeisterTests.GetDatenMeisterScope();
        var viewLogic = datenMeister.Resolve<FormMethods>();
        var internalViewExtent = viewLogic.GetInternalFormExtent();

        Assert.That(internalViewExtent, Is.Not.Null);
    }

    [Test]
    public async Task TestAvailabilityOfUserViews()
    {
        var datenMeister = await DatenMeisterTests.GetDatenMeisterScope();
        var viewLogic = datenMeister.Resolve<FormMethods>();
        var userViewExtent = viewLogic.GetUserFormExtent();

        Assert.That(userViewExtent, Is.Not.Null);
    }

    [Test]
    public async Task TestGetAllViews()
    {
        var datenMeister = await DatenMeisterTests.GetDatenMeisterScope();
        var viewLogic = datenMeister.Resolve<FormMethods>();
        var viewExtent = viewLogic.GetUserFormExtent();
        var factory = new MofFactory(viewExtent);
            
        var listForm = _Forms.TheOne.__TableForm;
            
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