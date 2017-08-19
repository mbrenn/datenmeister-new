using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.UserManagement;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class UserManagementTests
    {
        [Test]
        public void TestCreationOfUser()
        {
            var datenMeister = GiveMe.DatenMeister();
            var userLogic = datenMeister.Resolve<UserLogic>();

            userLogic.AddUser("mb", "test");
            Assert.That(userLogic.VerifyUser("mb", "test"), Is.True);
            Assert.That(userLogic.VerifyUser("mb", "tst"), Is.False);
            Assert.That(userLogic.VerifyUser("ab", "tst"), Is.False);

            userLogic.ChangePassword("mb", "tst");
            Assert.That(userLogic.VerifyUser("mb", "test"), Is.False);
            Assert.That(userLogic.VerifyUser("mb", "tst"), Is.True);
            Assert.That(userLogic.VerifyUser("ab", "tst"), Is.False);

        }
        
    }
}