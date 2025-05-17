using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using NUnit.Framework;
using StundenMeister.Logic;
using StundenMeister.Model;

namespace StundenMeister.Tests
{
    public class TimeRecordingTests
    {
        [Test]
        public void TestDeterminationOfWeek()
        {
            using (BootUp.CreateDatenMeisterEnvironment())
            {
                var logic = StundenMeisterPlugin.Get();
                Assert.That(logic, Is.Not.Null);

                var timeRecordingLogic = new TimeRecordingLogic(logic);
                var startOfWeek = timeRecordingLogic.FindStartOfWeek(new DateTime(2019, 07, 30));

                Assert.That(startOfWeek, Is.EqualTo(
                    new DateTime(2019,07,29)));

                startOfWeek = timeRecordingLogic.FindStartOfWeek(new DateTime(2019, 07, 29));
                Assert.That(startOfWeek, Is.EqualTo(
                    new DateTime(2019,07,29)));


                startOfWeek = timeRecordingLogic.FindStartOfWeek(new DateTime(2019, 07, 31));
                Assert.That(startOfWeek, Is.EqualTo(
                    new DateTime(2019,07,29)));


                startOfWeek = timeRecordingLogic.FindStartOfWeek(new DateTime(2019, 08, 04));
                Assert.That(startOfWeek, Is.EqualTo(
                    new DateTime(2019,07,29)));

                var endOfWeek = timeRecordingLogic.FindEndOfWeek(new DateTime(2019, 08, 04));
                Assert.That(endOfWeek, Is.EqualTo(
                    new DateTime(2019,08,05)));

                endOfWeek = timeRecordingLogic.FindEndOfWeek(new DateTime(2019, 07, 29));
                Assert.That(endOfWeek, Is.EqualTo(
                    new DateTime(2019,08,05)));
            }
        }

        [Test]
        public void TestDeterminationOfMonth()
        {
            using (BootUp.CreateDatenMeisterEnvironment())
            {
                var logic = StundenMeisterPlugin.Get();
                Assert.That(logic, Is.Not.Null);

                var timeRecordingLogic = new TimeRecordingLogic(logic);

                var date = new DateTime(2018, 11, 10);
                var startDate = timeRecordingLogic.FindStartOfMonth(date);

                Assert.That(startDate, Is.EqualTo(new DateTime(2018, 11, 1)));

                date = new DateTime(2018, 12, 10);
                startDate = timeRecordingLogic.FindStartOfMonth(date);

                Assert.That(startDate, Is.EqualTo(new DateTime(2018, 12, 1)));


                date = new DateTime(2018, 11, 10);
                var endDate = timeRecordingLogic.FindEndOfMonth(date);

                Assert.That(endDate, Is.EqualTo(new DateTime(2018, 12, 01)));

                date = new DateTime(2018, 12, 10);
                endDate = timeRecordingLogic.FindEndOfMonth(date);

                Assert.That(endDate, Is.EqualTo(new DateTime(2019, 01, 01)));
            }
        }

        [Test]
        public void TestRemovalOfMultipleActivations()
        {
            using (BootUp.CreateDatenMeisterEnvironment())
            {
                var logic = StundenMeisterPlugin.Get();
                var timeRecordingLogic = new TimeRecordingLogic(logic);

                var factory = new MofFactory(logic.Data.Extent);

                var recording1 = factory.create(logic.Data.ClassTimeRecording);
                var recording2 = factory.create(logic.Data.ClassTimeRecording);
                var recording3 = factory.create(logic.Data.ClassTimeRecording);
                var recording4 = factory.create(logic.Data.ClassTimeRecording);

                recording1.SetProperties(new Dictionary<string, object>
                {
                    [nameof(TimeRecording.startDate)] = new DateTime(2019, 10, 05, 12, 00, 00),
                    [nameof(TimeRecording.endDate)] = new DateTime(2019, 10, 05, 18, 00, 00),
                    [nameof(TimeRecording.isActive)] = true
                });

                recording2.SetProperties(new Dictionary<string, object>
                {
                    [nameof(TimeRecording.startDate)] = new DateTime(2019, 10, 06, 12, 00, 00),
                    [nameof(TimeRecording.endDate)] = new DateTime(2019, 10, 06, 18, 00, 00),
                    [nameof(TimeRecording.isActive)] = true
                });

                recording3.SetProperties(new Dictionary<string, object>
                {
                    [nameof(TimeRecording.startDate)] = new DateTime(2019, 10, 07, 12, 00, 00),
                    [nameof(TimeRecording.endDate)] = new DateTime(2019, 10, 07, 18, 00, 00),
                    [nameof(TimeRecording.isActive)] = true
                });

                recording4.SetProperties(new Dictionary<string, object>
                {
                    [nameof(TimeRecording.startDate)] = new DateTime(2019, 10, 08, 12, 00, 00),
                    [nameof(TimeRecording.endDate)] = new DateTime(2019, 10, 08, 18, 00, 00),
                    [nameof(TimeRecording.isActive)] = true
                });

                logic.Data.Extent.elements().add(recording1);
                logic.Data.Extent.elements().add(recording2);
                logic.Data.Extent.elements().add(recording3);
                logic.Data.Extent.elements().add(recording4);

                var numberActive = logic.Data.Extent.elements().OfType<IElement>()
                    .Count(x => x.get<bool>(nameof(TimeRecording.isActive)));
                Assert.That(numberActive, Is.EqualTo(4));

                timeRecordingLogic.Initialize();

                numberActive = logic.Data.Extent.elements().OfType<IElement>()
                    .Count(x => x.get<bool>(nameof(TimeRecording.isActive)));
                Assert.That(numberActive, Is.EqualTo(1));
                Assert.That(recording4.get<bool>(nameof(TimeRecording.isActive)), Is.True);
                Assert.That(recording3.get<bool>(nameof(TimeRecording.isActive)), Is.False);


            }
        }

    }
}