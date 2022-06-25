using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Users.UserProperties;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.UserProperties
{
    [TestFixture]
    public class UserPropertyTests
    {
        [Test]
        public void TestUserPropertiesViewMode()
        {
            var userProperty = new UserPropertyData();
            var result = userProperty.GetViewModeSelection("dm:///");
            Assert.That(result, Is.Null);

            var viewMode1 = InMemoryObject.CreateEmpty();
            var viewMode2 = InMemoryObject.CreateEmpty();
            var viewMode3 = InMemoryObject.CreateEmpty();
            
            userProperty.AddViewModeSelection("dm:///12", viewMode1);
            userProperty.AddViewModeSelection("dm:///12", viewMode2);
            userProperty.AddViewModeSelection("dm:///123", viewMode1);
            
            result = userProperty.GetViewModeSelection("dm:///12");
            Assert.That(result, Is.EqualTo(viewMode2));
            result = userProperty.GetViewModeSelection("dm:///123");
            Assert.That(result, Is.EqualTo(viewMode1));
            
            
            userProperty.AddViewModeSelection("dm:///123", viewMode3, "hello");
            
            result = userProperty.GetViewModeSelection("dm:///123");
            Assert.That(result, Is.EqualTo(viewMode1));
            
            result = userProperty.GetViewModeSelection("dm:///123", "hello");
            Assert.That(result, Is.EqualTo(viewMode3));
        }
    }
}