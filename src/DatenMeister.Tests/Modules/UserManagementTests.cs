using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.UserManagement;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules
{
    [TestFixture]
    public class UserManagementTests
    {
        public void TestCreationOfUser()
        {
            var datenMeister = GiveMe.DatenMeister();
            var userLogic = datenMeister.Resolve<UserLogic>();
            
        }
        
    }
}