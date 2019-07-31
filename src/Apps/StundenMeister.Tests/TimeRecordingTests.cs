
using System;
using NUnit.Framework;
using StundenMeister.Logic;

namespace StundenMeister.Tests
{
    public class TimeRecordingTests
    {
        [Test]
        public void TestDeterminationOfWeek()
        {
            using (BootUp.CreateDatenMeisterEnvironment())
            {
                var logic = StundenMeisterLogic.Get();
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
                    new DateTime(2019,07,05)));
                
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
                var logic = StundenMeisterLogic.Get();
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
        
    }
}