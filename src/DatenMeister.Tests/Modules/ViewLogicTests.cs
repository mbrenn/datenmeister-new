using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ViewFinder;
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
            var viewLogic = datenMeister.Resolve<ViewLogic>();
            var internalViewExtent = viewLogic.GetInternalViewExtent();

            Assert.That(internalViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestAvailabiltyOfUserViews()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<ViewLogic>();
            var userViewExtent = viewLogic.GetUserViewExtent();

            Assert.That(userViewExtent, Is.Not.Null);
        }

        [Test]
        public void TestGetAllViews()
        {
            var datenMeister = GiveMe.DatenMeister();
            var viewLogic = datenMeister.Resolve<ViewLogic>();
            var n = 0;
            foreach (var view in viewLogic.GetAllViews())
            {
                n++;
            }

            Assert.That(n, Is.GreaterThan(0));
        }
    }
}