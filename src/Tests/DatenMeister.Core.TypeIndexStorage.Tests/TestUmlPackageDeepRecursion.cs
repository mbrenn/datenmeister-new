using System;
using System.Linq;
using System.Threading.Tasks;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.TypeIndexAssembly;
using NUnit.Framework;

namespace DatenMeister.Core.TypeIndexStorage.Tests;

[TestFixture]
public class TestUmlPackageDeepRecursion
{
    [Test]
    public async Task TestPackageHasAttributeName()
    {
        await using var dm = await IntegrationOfTests.GetDatenMeisterScope();
        var typeIndexStore = dm.ScopeStorage.TryGet<TypeIndexStore>()
                             ?? throw new InvalidOperationException("TypeIndexStore not found");
        
        var typesWorkspace = typeIndexStore.GetCurrentIndexStore().FindWorkspace(WorkspaceNames.WorkspaceUml);
        Assert.That(typesWorkspace, Is.Not.Null);

        // Find the "Package" class. 
        // In UML, Package inherits from NamedElement (usually via multiple levels)
        var packageClass = typesWorkspace!.ClassModels
            .FirstOrDefault(x => x.Name == "Package");
        
        Assert.That(packageClass, Is.Not.Null, "Package class not found in Uml workspace");

        // Check if it has the "name" attribute.
        // Package -> Namespace -> NamedElement (which has 'name')
        var nameAttribute = packageClass!.Attributes.FirstOrDefault(x => x.Name == "name");
        
        Assert.That(nameAttribute, Is.Not.Null, "Attribute 'name' not found in Package class");
        
        // Let's check for 'packagedElement' which is inherited from Package to its subpackages, 
        // but here it is a direct attribute of Package.
        var packagedElementAttribute = packageClass.Attributes.FirstOrDefault(x => x.Name == "packagedElement");
        Assert.That(packagedElementAttribute, Is.Not.Null, "Attribute 'packagedElement' not found in Package class");
        
        // Verify that we have some attributes that are inherited.
        // Property inherits from StructuralFeature -> Feature -> RedefinableElement -> NamedElement
        var propertyClass = typesWorkspace.ClassModels.FirstOrDefault(x => x.Name == "Property");
        Assert.That(propertyClass, Is.Not.Null, "Class 'Property' not found");
        var propertyNameAttribute = propertyClass!.Attributes.FirstOrDefault(x => x.Name == "name");
        Assert.That(propertyNameAttribute, Is.Not.Null, "Attribute 'name' not found in Class 'Property'");
        Assert.That(propertyNameAttribute!.IsInherited, Is.True, "Attribute 'name' should be inherited for Class 'Property'");
    }
}
