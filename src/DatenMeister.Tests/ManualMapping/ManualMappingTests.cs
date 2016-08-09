using DatenMeister.ManualMapping;
using NUnit.Framework;

namespace DatenMeister.Tests.ManualMapping
{
    [TestFixture]
    public class ManualMappingTests
    {
        [Test]
        public void TestMMObjects()
        {
            var typeMapper = new TypeMapping();
            typeMapper.CreateNewObject = () => new MapTestClass();
            typeMapper.GetId = x => (x as MapTestClass).Id.ToString();
            typeMapper.AddProperty(
                MapTestClass.IdProperty,
                x => (x as MapTestClass).Id,
                (x, v) => (x as MapTestClass).Id = v);
            typeMapper.AddProperty(
                MapTestClass.NameProperty,
                x => (x as MapTestClass).Name,
                (x, v) => (x as MapTestClass).Name = v);

        }

        public class MapTestClass
        {
            public string Name{get; set; }
            public int Id { get; set; }

            public static object IdProperty { get; } = new object();
            public static object NameProperty { get; } = new object();
        }
    }
}