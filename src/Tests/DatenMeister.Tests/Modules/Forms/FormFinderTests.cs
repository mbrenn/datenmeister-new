using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.Helper;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Forms;

[TestFixture]
public class FormFinderTests
{
    [Test]
    public async Task TestCheckByMetaClass()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var formMethods = new FormMethods(dm.WorkspaceLogic);
        var formFinder = new FormFinder(formMethods);

        var classInstance = dm.WorkspaceLogic.FindElement(
            WorkspaceNames.WorkspaceUml,
            _UML.TheOne.StructuredClassifiers.__Class.Uri);
        Assert.That(classInstance, Is.Not.Null);
        var classifierInstance = dm.WorkspaceLogic.FindElement(
            WorkspaceNames.WorkspaceUml,
            _UML.TheOne.Classification.__Classifier.Uri);
        Assert.That(classifierInstance, Is.Not.Null);

        var query = new FindFormQuery
        {
            MetaClass = classInstance,
            FormType = _Forms._FormTypes.___FormType.Object
        };

        var form = InMemoryObject.CreateEmpty(_Forms.TheOne.FormTypes.__ObjectForm);

        var formAssociation = InMemoryObject.CreateEmpty(_Forms.TheOne.FormTypes.__FormAssociation);
        formAssociation.set(_Forms._FormTypes._FormAssociation.metaClass, classInstance);
        formAssociation.set(_Forms._FormTypes._FormAssociation.form, form);
        formAssociation.set(_Forms._FormTypes._FormAssociation.formType, query.FormType);

        var points = formFinder.EvaluateFormAssociation(query, formAssociation, []);
        Assert.That(points, Is.GreaterThan(-1));
        
        // Now check the generalization are not caught
        formAssociation.set(_Forms._FormTypes._FormAssociation.metaClass, classifierInstance);

        var points2 = formFinder.EvaluateFormAssociation(query, formAssociation, []);
        Assert.That(points2, Is.EqualTo(-1));
    }
    
    [Test]
    public async Task TestCheckByMetaClassWithGeneralization()
    {
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var formMethods = new FormMethods(dm.WorkspaceLogic);
        var formFinder = new FormFinder(formMethods);

        var classInstance = dm.WorkspaceLogic.FindElement(
            WorkspaceNames.WorkspaceUml,
            _UML.TheOne.StructuredClassifiers.__Class.Uri);
        Assert.That(classInstance, Is.Not.Null);
        var classifierInstance = dm.WorkspaceLogic.FindElement(
            WorkspaceNames.WorkspaceUml,
            _UML.TheOne.Classification.__Classifier.Uri);
        Assert.That(classifierInstance, Is.Not.Null);

        var query = new FindFormQuery
        {
            MetaClass = classInstance,
            FormType = _Forms._FormTypes.___FormType.Object
        };

        var form = InMemoryObject.CreateEmpty(_Forms.TheOne.FormTypes.__ObjectForm);

        var formAssociation = InMemoryObject.CreateEmpty(_Forms.TheOne.FormTypes.__FormAssociation);
        formAssociation.set(_Forms._FormTypes._FormAssociation.metaClass, classInstance);
        formAssociation.set(_Forms._FormTypes._FormAssociation.form, form);
        formAssociation.set(_Forms._FormTypes._FormAssociation.includeGeneralization, true);
        formAssociation.set(_Forms._FormTypes._FormAssociation.formType, query.FormType);

        var points = formFinder.EvaluateFormAssociation(query, formAssociation, []);
        Assert.That(points, Is.GreaterThan(-1));
        
        // Now check the generalization are not caught
        formAssociation.set(_Forms._FormTypes._FormAssociation.metaClass, classifierInstance);

        var points2 = formFinder.EvaluateFormAssociation(query, formAssociation, []);
        Assert.That(points2, Is.GreaterThan(-1));
    }
    
}