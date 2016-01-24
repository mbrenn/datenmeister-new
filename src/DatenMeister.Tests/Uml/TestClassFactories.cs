using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Tests.Xmi;
using DatenMeister.Uml.TypeFactory;
using NUnit.Framework;

namespace DatenMeister.Tests.Uml
{
    [TestFixture]
    public class TestClassFactories
    {
        [Test]
        public void TestCreateType()
        {
            var factory = new MofFactory();

            XmiTests.CreateUmlAndMofInstance(out _MOF.TheOne, out _UML.TheOne);

            var typeCreator = new TypeFactoryDotNet(_UML.TheOne, factory);
            var typeWorkspace = typeof (Workspace<IExtent>);

            var type = typeCreator.CreateFromDotNetType(typeWorkspace);
            Assert.That(type, Is.Not.Null);
        }
    }
}