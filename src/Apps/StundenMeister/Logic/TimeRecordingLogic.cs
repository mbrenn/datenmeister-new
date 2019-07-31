using System;
using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using StundenMeister.Model;

namespace StundenMeister.Logic
{
    public class TimeRecordingLogic
    {
        private StundenMeisterLogic _stundenMeisterLogic;

        public TimeRecordingLogic(StundenMeisterLogic stundenMeisterLogic)
        {
            _stundenMeisterLogic = stundenMeisterLogic;
        }

        /// <summary>
        /// Creates a new element containing the time recordings
        /// </summary>
        /// <returns>The element being created</returns>
        public IElement CreateAndAddNewTimeRecoding()
        {
            var factory = new MofFactory(StundenMeisterData.TheOne.Extent);
            var createdItem = factory.create(StundenMeisterData.TheOne.ClassTimeRecording);
            StundenMeisterData.TheOne.Extent.elements().add(createdItem);
            _stundenMeisterLogic.Data.CurrentTimeRecording = createdItem;
            return createdItem;
        }

        /// <summary>
        /// Starts a new recording of timing.
        /// Ends the current one and creates a new time record
        /// </summary>
        public void StartNewRecording()
        {
            EndRecording();

            var logic = new TimeRecordingLogic(StundenMeisterLogic.Get());

            var currentTimeRecording = logic.CreateAndAddNewTimeRecoding();
            currentTimeRecording.set(nameof(TimeRecording.startDate), DateTime.UtcNow);
            currentTimeRecording.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
            currentTimeRecording.set(nameof(TimeRecording.isActive), true);
            
        }

        /// <summary>
        /// Ends the current active recording 
        /// </summary>
        public void EndRecording()
        {
            var currentTimeRecording = _stundenMeisterLogic.Data.CurrentTimeRecording;
            
            if (currentTimeRecording != null)
            {
                currentTimeRecording.set(nameof(TimeRecording.isActive), false);
                currentTimeRecording.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
                _stundenMeisterLogic.Data.CurrentTimeRecording = null;
            }
        }

        /// <summary>
        /// Checks whether the time recording is active
        /// </summary>
        /// <returns>true, if the time recording is active</returns>
        public bool IsTimeRecordingActive()
        {
            var currentTimeRecording = StundenMeisterData.TheOne.CurrentTimeRecording;
            return currentTimeRecording != null && currentTimeRecording.getOrDefault<bool>(nameof(TimeRecording.isActive));
        }

        /// <summary>
        /// Updates the timing of the current recording
        /// </summary>
        public void UpdateCurrentRecording()
        {
            var currentTimeRecording = StundenMeisterData.TheOne.CurrentTimeRecording;
            // There is an active recording. We have to update the information to show the user the latest
            // and greatest information. 
            if (currentTimeRecording == null)
            {
                // Nothing to do... no active recording
                return;
            }
            
            var isActive = currentTimeRecording.getOrDefault<bool>(nameof(TimeRecording.isActive));
                
            if (isActive)
            {
                // While current time recording is active, advance content
                var endDate = DateTime.UtcNow;
                currentTimeRecording.set(nameof(TimeRecording.endDate), endDate);
            }
        }

        public TimeSpan CalculateWorkingHoursInDay(DateTime day = default)
        {
            if (day == default)
            {
                day = DateTime.Now;
            }
            
            var startDate = FindStartOfDay(day);
            var endDate = FindEndOfDay(day);

            return CalculateWorkingHoursInPeriod(startDate, endDate);
        }

        /// <summary>
        /// Calculates the working hours in the current week
        /// </summary>
        /// <param name="day">Day to be evaluated</param>
        /// <returns>Timespan storing the information</returns>
        public TimeSpan CalculateWorkingHoursInWeek(DateTime day = default)
        {
            if (day == default)
            {
                day = DateTime.Now;
            }
            
            var startDate = FindStartOfWeek(day);
            var endDate = FindEndOfWeek(day);

            return CalculateWorkingHoursInPeriod(startDate, endDate);
        }

        public TimeSpan CalculateWorkingHoursInMonth(DateTime day = default)
        {
            if (day == default)
            {
                day = DateTime.Now;
            }
            
            var startDate = FindStartOfMonth(day);
            var endDate = FindEndOfMonth(day);

            return CalculateWorkingHoursInPeriod(startDate, endDate);
        }

        private TimeSpan CalculateWorkingHoursInPeriod(DateTime startDate, DateTime endDate)
        {
            var result = TimeSpan.Zero;

            foreach (var recording in _stundenMeisterLogic.Data.Extent
                .elements()
                .WhenMetaClassIs(_stundenMeisterLogic.Data.ClassTimeRecording)
                .OfType<IElement>())
            {
                var recordingStartDate = recording.getOrDefault<DateTime>(nameof(TimeRecording.startDate));
                if (recordingStartDate >= startDate && recordingStartDate < endDate)
                {
                    result = result.Add(GetTimeSpanOfRecording(recording));
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the start of the day
        /// </summary>
        /// <param name="day">Day to be used</param>
        /// <returns>Date of the start of the date (inclusive)</returns>
        public DateTime FindStartOfDay(DateTime day)
        {
            return new DateTime(day.Year, day.Month, day.Day);
        }

        /// <summary>
        /// Finds the end of the day
        /// </summary>
        /// <param name="day">Day to be used</param>
        /// <returns>Date of the end of the date (non-inclusive)</returns>
        public DateTime FindEndOfDay(DateTime day)
        {
            return new DateTime(day.Year, day.Month, day.Day).AddDays(1);
        }

        public DateTime FindStartOfWeek(DateTime day)
        {
            var dayOfWeek = day.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-6);
                case DayOfWeek.Monday:
                    return new DateTime(day.Year, day.Month,day.Day);
                case DayOfWeek.Tuesday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-1);
                case DayOfWeek.Wednesday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-2);
                case DayOfWeek.Thursday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-3);
                case DayOfWeek.Friday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-4);
                case DayOfWeek.Saturday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(-5);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DateTime FindEndOfWeek(DateTime day)
        {
            var dayOfWeek = day.DayOfWeek;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(1);
                case DayOfWeek.Monday:
                    return new DateTime(day.Year, day.Month,day.Day).AddDays(7);
                case DayOfWeek.Tuesday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(6);
                case DayOfWeek.Wednesday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(5);
                case DayOfWeek.Thursday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(4);
                case DayOfWeek.Friday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(3);
                case DayOfWeek.Saturday:
                    return new DateTime(day.Year, day.Month, day.Day).AddDays(2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Finds the first day of the month
        /// </summary>
        /// <param name="day">Day of which the first day in month is looked up for</param>
        /// <returns>The found date</returns>
        public DateTime FindStartOfMonth(DateTime day)
        {
            return new DateTime(day.Year, day.Month, 1);
        }

        /// <summary>
        /// Finds the last day of the month. (The first position in the next year will be returned)
        /// </summary>
        /// <param name="day">Day of which the last day in month is looked up</param>
        /// <returns>The found date</returns>
        public DateTime FindEndOfMonth(DateTime day)
        {
            if (day.Month == 12)
            {
                return new DateTime(day.Year + 1, 1, 1);
            }

            return new DateTime(day.Year, day.Month + 1, 1);
        }

        /// <summary>
        /// Gets the timespan of the recording
        /// </summary>
        /// <param name="timeRecording">Recording to be evaluated</param>
        /// <returns>The difference between startDate and endDate</returns>
        public TimeSpan GetTimeSpanOfRecording(IElement timeRecording)
        {
            var startDate = timeRecording.getOrDefault<DateTime>(nameof(TimeRecording.startDate));
            var endDate = timeRecording.getOrDefault<DateTime>(nameof(TimeRecording.endDate));

            return endDate - startDate;
        }
    }
}