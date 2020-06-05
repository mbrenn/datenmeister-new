using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.AutoEnumerate;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Modules.ZipExample;
using DatenMeister.Provider.CSV.Runtime;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Plugin;
using NUnit.Framework;

namespace DatenMeister.Tests.Runtime.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [Test]
        public void ExtentTest()
        {
            using var builder = DatenMeisterTests.GetDatenMeisterScope();
            
            using var scope = builder.BeginLifetimeScope();
            var workspaceLogic = scope.Resolve<IWorkspaceLogic>();
            var workspaceExtent = workspaceLogic.FindExtent(WorkspaceNames.UriExtentWorkspaces);
            Assert.That(workspaceExtent, Is.Not.Null);
            var asData = workspaceExtent.elements().Cast<IElement>()
                .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceData);
            var asManagement = workspaceExtent.elements().Cast<IElement>()
                .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceManagement);
            var asTypes = workspaceExtent.elements().Cast<IElement>()
                .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceTypes);
            var asMof = workspaceExtent.elements().Cast<IElement>()
                .First(x => x.get("id")?.ToString() == WorkspaceNames.WorkspaceMof);

            Assert.That(asData, Is.Not.Null);
            Assert.That(asManagement, Is.Not.Null);
            Assert.That(asTypes, Is.Not.Null);
            Assert.That(asMof, Is.Not.Null);

            // Get the extents
            var extents = (asMof.get("extents") as IEnumerable<object>)?.ToList();
            Assert.That(extents, Is.Not.Null);

            var mofExtent = extents.Cast<IElement>().First(x => x.get("uri")?.ToString() == WorkspaceNames.UriExtentMof);
            Assert.That(mofExtent, Is.Not.Null);
        }

        [Test]
        public void TestMetaDataInExtent()
        {
            var path = "./test.xmi";
            var loaderConfig = new XmiStorageLoaderConfig("dm:///data")
            {
                filePath = path,
                workspaceId = WorkspaceNames.WorkspaceData
            };

            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                var extentLoader = dm.Resolve<IExtentManager>();
                var loadedExtent = extentLoader.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);
                loadedExtent.set("test", "this is a test");
                loadedExtent.GetConfiguration().ExtentType = "Happy Extent";
                extentLoader.StoreExtent(loadedExtent);

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(dropDatabase: false))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var foundExtent = workspaceLogic.FindExtent("dm:///data");
                Assert.That(foundExtent, Is.Not.Null);

                Assert.That(foundExtent.get("test"), Is.EqualTo("this is a test"));
                Assert.That(foundExtent.GetConfiguration().ExtentType, Is.EqualTo("Happy Extent"));

                dm.UnuseDatenMeister();
            }
        }

        [Test]
        public void TestDefaultExtentType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
            var typesWorkspace = workspaceLogic.GetTypesWorkspace();
            var zipCodeModel =
                typesWorkspace.FindElementByUri("dm:///_internal/types/internal?fn=" +
                                                ZipCodeModel.PackagePath);

            var dataWorkspace = workspaceLogic.GetDataWorkspace();

            var zipExample = zipCodeExample.AddZipCodeExample(dataWorkspace);
            var setDefaultTypePackage = zipExample.GetConfiguration().GetDefaultTypePackages()?.ToList();

            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(zipCodeModel, Is.Not.Null);

            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));
        }

        [Test]
        public void TestExtentSettingsExtentType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extentSettings = dm.Resolve<ExtentSettings>();
            Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == UmlPlugin.ExtentType), Is.True);
            Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == FormsPlugin.FormExtentType), Is.True);
            Assert.That(extentSettings.extentTypeSettings.Any(x => x.name == ZipCodePlugin.ExtentType), Is.True);
        }

        [Test]
        public void TestAutoEnumerateType()
        {
            var path = "./test.xmi";
            var loaderConfig = new XmiStorageLoaderConfig("dm:///data")
            {
                filePath = path,
                workspaceId = WorkspaceNames.WorkspaceData
            };

            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                var extentLoader = dm.Resolve<IExtentManager>();
                var loadedExtent = extentLoader.LoadExtent(loaderConfig, ExtentCreationFlags.LoadOrCreate);
                loadedExtent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Ordinal;
                extentLoader.StoreExtent(loadedExtent);

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(dropDatabase: false))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var foundExtent = workspaceLogic.FindExtent("dm:///data");
                Assert.That(foundExtent, Is.Not.Null);
                Assert.That(foundExtent.GetConfiguration().AutoEnumerateType, Is.EqualTo(AutoEnumerateType.Ordinal));
                
                foundExtent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Guid;
                Assert.That(foundExtent.GetConfiguration().AutoEnumerateType, Is.EqualTo(AutoEnumerateType.Guid));

                dm.UnuseDatenMeister();
            }
        }

        [Test]
        public void TestAddDefaultExtentType()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
            var zipCodeExample = dm.Resolve<ZipCodeExampleManager>();
            var typesWorkspace = workspaceLogic.GetTypesWorkspace();
            var zipCodeModel =
                typesWorkspace.FindElementByUri("dm:///_internal/types/internal?fn=" +
                                                ZipCodeModel.PackagePath) as IElement;
            Assert.That(zipCodeModel, Is.Not.Null);

            var dataWorkspace = workspaceLogic.GetDataWorkspace();

            var zipExample = zipCodeExample.AddZipCodeExample(dataWorkspace);

            // Per Default, one is included
            var setDefaultTypePackage = zipExample.GetConfiguration().GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));

            // Checks, if adding another one does not work
            zipExample.GetConfiguration().AddDefaultTypePackages(new[] {zipCodeModel});
            setDefaultTypePackage = zipExample.GetConfiguration().GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));

            // Checks, if removing works
            zipExample.GetConfiguration().SetDefaultTypePackages(new IElement[] { });
            setDefaultTypePackage = zipExample.GetConfiguration().GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(0));

            // Checks, if adding works now correctly
            zipExample.GetConfiguration().AddDefaultTypePackages(new[] {zipCodeModel});
            setDefaultTypePackage = zipExample.GetConfiguration().GetDefaultTypePackages()?.ToList();
            Assert.That(setDefaultTypePackage, Is.Not.Null);
            Assert.That(setDefaultTypePackage.Count, Is.EqualTo(1));
            Assert.That(setDefaultTypePackage.FirstOrDefault(), Is.EqualTo(zipCodeModel));
        }

        [Test]
        public void TestExtentTypes()
        {
            var extent = new MofUriExtent(new InMemoryProvider());
            var configuration = new ExtentConfiguration(extent);

            configuration.ExtentType = "abc";
            
            var list = configuration.ExtentTypes.ToList();
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0], Is.EqualTo("abc"));
            
            configuration.ExtentType = "abc def";
            list = configuration.ExtentTypes.ToList();
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("abc"));
            Assert.That(list[1], Is.EqualTo("def"));

            configuration.ExtentTypes = new[] {"abc", "def"};
            Assert.That(configuration.ExtentType, Is.EqualTo("abc def"));
            list = configuration.ExtentTypes.ToList();
            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0], Is.EqualTo("abc"));
            Assert.That(list[1], Is.EqualTo("def"));
        }

        [Test]
        public void TestStoringOfExtentTypes()
        {
            const string csvExtentUri = "dm:///csvtest";
            const string xmiExtentUri = "dm:///moftest";

            using (var dm = DatenMeisterTests.GetDatenMeisterScope())
            {
                var extentManager = dm.Resolve<IExtentManager>();
                var csvExtent = extentManager.LoadExtent(
                    new CsvExtentLoaderConfig(csvExtentUri)
                    {
                        filePath = "./test.csv"
                    }, ExtentCreationFlags.LoadOrCreate);

                var mofExtent = extentManager.LoadExtent(
                    new XmiStorageLoaderConfig(xmiExtentUri)
                    {
                        filePath = "./test.xmi"
                    }, ExtentCreationFlags.LoadOrCreate);

                csvExtent.GetConfiguration().ExtentType = "CSVExtent";
                mofExtent.GetConfiguration().ExtentType = "XMIExtent";

                dm.UnuseDatenMeister();
            }

            using (var dm = DatenMeisterTests.GetDatenMeisterScope(false))
            {
                var workspaceLogic = dm.Resolve<IWorkspaceLogic>();
                var csvExtent = workspaceLogic.FindExtent(csvExtentUri);
                var xmiExtent = workspaceLogic.FindExtent(xmiExtentUri);

                Assert.That(csvExtent, Is.Not.Null);
                Assert.That(xmiExtent, Is.Not.Null);

                Assert.That(csvExtent.GetConfiguration().ExtentType, Is.EqualTo("CSVExtent"));
                Assert.That(xmiExtent.GetConfiguration().ExtentType, Is.EqualTo("XMIExtent"));

                dm.UnuseDatenMeister();
            }
        }

        [Test]
        public static void TestAlternativeUris()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///a");
            mofExtent.AlternativeUris.Add("dm:///test");
            mofExtent.AlternativeUris.Add("dm:///test2");

            Assert.That(mofExtent.AlternativeUris.Count, Is.EqualTo(2));
            Assert.That(mofExtent.AlternativeUris.Contains("dm:///test"), Is.True);
            Assert.That(mofExtent.AlternativeUris.Contains("dm:///test2"), Is.True);
        }

        [Test]
        public static void TestAutoSettingOfIds()
        {
            var mofExtent = new MofUriExtent(new InMemoryProvider(), "dm:///a");
            var mofFactory = new MofFactory(mofExtent);

            var element1 = mofFactory.create(null);
            mofExtent.elements().add(element1);
            var element2 = mofFactory.create(null);
            mofExtent.elements().add(element2);

            var n1 = ExtentHelper.SetAvailableId(mofExtent, element1, "name");
            var n2 = ExtentHelper.SetAvailableId(mofExtent, element2, "name");

            Assert.That(n1, Is.EqualTo("name"));
            Assert.That(n2, Is.EqualTo("name-1"));

            Assert.That((element1 as IHasId)?.Id, Is.EqualTo("name"));
            Assert.That((element2 as IHasId)?.Id, Is.Not.EqualTo("name"));
            Assert.That((element2 as IHasId)?.Id, Contains.Substring("name"));
        }

        [Test]
        public static void TestDefaultValue()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var extent = CreateType(dm, out var type);

            var extentFactory = new MofFactory(extent);
            
            var createdType = extentFactory.create(type);
            Assert.That(createdType, Is.Not.Null);
            Assert.That(createdType.getOrDefault<int>("age"), Is.EqualTo(18));
        }

        [Test]
        public static void TestAutoEnumerateGuidProperty()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            
            var extent = CreateType(dm, out var type);

            var extentFactory = new MofFactory(extent);

            extent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Guid;
            var createdType = extentFactory.create(type);
            Assert.That(createdType, Is.Not.Null);
            Assert.That(createdType.getOrDefault<string>("id"), Is.Not.Null);

            var id = createdType.getOrDefault<string>("id");
            Assert.That(id.Length, Is.EqualTo(36));
                
                
            var createdType2 = extentFactory.create(type);
            Assert.That(createdType2, Is.Not.Null);
            Assert.That(createdType2.getOrDefault<string>("id"), Is.Not.Null);

            var id2 = createdType2.getOrDefault<string>("id");
            Assert.That(id2.Length, Is.EqualTo(36));
            Assert.That(id2, Is.Not.EqualTo(id));
            var setId= (createdType2 as IHasId)?.Id;
            Assert.That(setId, Is.EqualTo(id2));
        }

        [Test]
        public static void TestAutoEnumerateOrdinalProperty()
        {
            using var dm = DatenMeisterTests.GetDatenMeisterScope();
            var extent = CreateType(dm, out var type);

            var extentFactory = new MofFactory(extent);

            extent.GetConfiguration().AutoEnumerateType = AutoEnumerateType.Ordinal;
            var createdType = extentFactory.create(type);
            Assert.That(createdType, Is.Not.Null);
            Assert.That(createdType.getOrDefault<string>("id"), Is.Not.Null);

            var id = createdType.getOrDefault<int>("id");
            Assert.That(id, Is.EqualTo(1));
                
                
            var createdType2 = extentFactory.create(type);
            Assert.That(createdType2, Is.Not.Null);
            Assert.That(createdType2.getOrDefault<int>("id"), Is.Not.Null);

            var id2 = createdType2.getOrDefault<int>("id");
            Assert.That(id2, Is.EqualTo(2));
            var setId= (createdType2 as IHasId)?.Id;
            Assert.That(setId, Is.EqualTo(id2.ToString()));
            
            extent.unset(AutoEnumerateHandler.AutoEnumerateTypeValue);
            
            
            var createdType3 = extentFactory.create(type);
            Assert.That(createdType3, Is.Not.Null);
            Assert.That(createdType3.getOrDefault<int>("id"), Is.Not.Null);

            var id3 = createdType3.getOrDefault<int>("id");
            Assert.That(id3, Is.EqualTo(1));

            extent.elements().add(createdType);
            extent.elements().add(createdType2);
            extent.elements().add(createdType3);
            
            extent.unset(AutoEnumerateHandler.AutoEnumerateTypeValue);
            
            var createdType4 = extentFactory.create(type);
            Assert.That(createdType4, Is.Not.Null);
            Assert.That(createdType4.getOrDefault<int>("id"), Is.Not.Null);
            

            var id4 = createdType4.getOrDefault<int>("id");
            Assert.That(id4, Is.EqualTo(3));
        }

        private static MofExtent CreateType(IDatenMeisterScope dm, out IElement type)
        {
            var uml = dm.WorkspaceLogic.GetUmlData();
            var localTypeSupport = dm.Resolve<LocalTypeSupport>();
            var userTypes = localTypeSupport.GetUserTypeExtent();
            var factory = new MofFactory(userTypes);

            var extent = new MofUriExtent(new InMemoryProvider());

            type = factory.create(uml.StructuredClassifiers.__Class);
            var property1 = factory.create(uml.Classification.__Property);
            property1.set(_UML._Classification._Property.isID, true);
            property1.set(_UML._CommonStructure._NamedElement.name, "id");
            
            var property2 = factory.create(uml.Classification.__Property);
            property2.set(_UML._CommonStructure._NamedElement.name, "name");
            
            var property3 = factory.create(uml.Classification.__Property);
            property3.set(_UML._CommonStructure._NamedElement.name, "age");
            property3.set(_UML._Classification._Property.defaultValue, 18);

            type.set(_UML._StructuredClassifiers._StructuredClassifier.ownedAttribute,
                new[] {property1, property2, property3});
            type.set(_UML._CommonStructure._NamedElement.name, "your");

            userTypes.elements().add(type);
            return extent;
        }

        [Test]
        public void TestQueryItemById()
        {
            var uriExtent = CreateLittleExtent();
            var package1 = uriExtent.element("dm:///test#p1");
            Assert.That(package1, Is.Not.Null);
            Assert.That(package1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
                Is.EqualTo("package1"));
            
            
            var element1 = uriExtent.element("dm:///test#e1");
            Assert.That(element1, Is.Not.Null);
            Assert.That(element1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
                Is.EqualTo("element1"));
        }

        [Test]
        public void TestQueryItemByFullname()
        {
            var uriExtent = CreateLittleExtent();
            var package1 = uriExtent.element("dm:///test?fn=package1");
            Assert.That(package1, Is.Not.Null);
            Assert.That(package1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
                Is.EqualTo("package1"));
            
            var element1 = uriExtent.element("dm:///test?fn=package1::element1");
            Assert.That(element1, Is.Not.Null);
            Assert.That(element1.getOrDefault<string>(_UML._CommonStructure._NamedElement.name),
                Is.EqualTo("element1"));
        }

        /// <summary>
        /// Creates a little uriextent for testing
        /// </summary>
        /// <returns>Uri extent to be tested</returns>
        public IUriExtent CreateLittleExtent()
        {
            var uriExtent = new MofUriExtent(new InMemoryProvider(), "dm:///test");
            var factory = new MofFactory(uriExtent);

            var package1 = factory.create(null);
            var package2 = factory.create(null);
            var element1 = factory.create(null);

            (package1 as ICanSetId)!.Id = "p1";
            (package2 as ICanSetId)!.Id = "p2";
            (element1 as ICanSetId)!.Id = "e1";
            
            package1.set(_UML._CommonStructure._NamedElement.name, "package1");
            package2.set(_UML._CommonStructure._NamedElement.name, "package2");
            element1.set(_UML._CommonStructure._NamedElement.name, "element1");
            
            package1.set(_UML._Packages._Package.packagedElement, new[] {element1});

            uriExtent.elements().add(package1);
            uriExtent.elements().add(package2);
            uriExtent.elements().add(element1);

            return uriExtent;
        }
    }
}