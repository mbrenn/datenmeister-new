using StundenMeister.Model;

namespace StundenMeister.Logic;

public class TimeRecordingLogic
{
    private bool hibernationActive => _stundenMeisterPlugin.Configuration.HibernationDetectionActive;

    private TimeSpan hibernationTime => _stundenMeisterPlugin.Configuration.HibernationDetectionTime;

    private readonly StundenMeisterPlugin _stundenMeisterPlugin;

    public TimeRecordingLogic(StundenMeisterPlugin stundenMeisterPlugin)
    {
        _stundenMeisterPlugin = stundenMeisterPlugin;
    }

    /// <summary>
    /// Performs the initialization.
    /// 1), it is looked, whether one time recording is still active and if yes, this time recording
    /// is set within the <c>StundenMeisterData</c> instance.
    /// 2) If there are multiple activations of the time recordings, only the last one is kept active
    /// </summary>
    public void Initialize()
    {
        var list = _stundenMeisterPlugin.Data.Extent.elements()
            .WhenMetaClassIs(
                _stundenMeisterPlugin.Data.ClassTimeRecording)
            .WhenPropertyHasValue(
                nameof(TimeRecording.isActive), true)
            .OfType<IElement>()
            .ToList();
        IElement theActiveOne = null;
        if (list.Count > 1)
        {
            // Find the one with the highest starting date and deactivate the rest
            var highest = DateTime.MinValue;

            foreach (var item in list)
            {
                var date = item.getOrDefault<DateTime>(nameof(TimeRecording.startDate));
                if (date > highest)
                {
                    theActiveOne = item;
                    highest = date;
                }
            }

            // Now, deactivate everything, which is not the active one
            foreach (var item in list)
            {
                if (item == theActiveOne)
                {
                    continue;
                }

                item.set(nameof(TimeRecording.isActive), false);
            }
        }
        else
        {
            theActiveOne = list.FirstOrDefault();
        }

        if (theActiveOne != null)
        {
            _stundenMeisterPlugin.Data.CurrentTimeRecording = theActiveOne;
        }
    }

    public IEnumerable<TimeRecordingSet> GetTimeRecordingSets()
    {
        var now = DateTime.Now;
        var startDay = FindStartOfDay(now);
        var endDay = FindEndOfDay(now);
        var startWeek = FindStartOfWeek(now);
        var endWeek = FindEndOfWeek(now);
        var startMonth = FindStartOfMonth(now);
        var endMonth = FindEndOfMonth(now);

        var total = new TimeRecordingSet {Title = "Total"};
        var list = new List<TimeRecordingSet>();

        TimeRecordingSet costCenterSet = null;
            
        foreach (var recording in _stundenMeisterPlugin.Data.Extent
                     .elements()
                     .WhenMetaClassIs(_stundenMeisterPlugin.Data.ClassTimeRecording)
                     .OfType<IElement>())
        {
            // Checks, if cost center is in given list
            var costCenter = recording.getOrDefault<IElement>(nameof(TimeRecording.costCenter));
            if (costCenter != null)
            {
                costCenterSet = list.FirstOrDefault(x=>x.CostCenter?.equals(costCenter) == true);
                if (costCenterSet == null)
                {
                    costCenterSet = new TimeRecordingSet
                    {
                        Title = costCenter.getOrDefault<string>(nameof(CostCenter.id)),
                        CostCenter = costCenter
                    };

                    list.Add(costCenterSet);
                }
            }

            var recordingStartDate = recording.getOrDefault<DateTime>(nameof(TimeRecording.startDate));

            if (recordingStartDate >= startDay && recordingStartDate < endDay)
            {
                total.Day = total.Day.Add(GetTimeSpanOfRecording(recording));
                if (costCenterSet != null)
                {
                    costCenterSet.Day = costCenterSet.Day.Add(GetTimeSpanOfRecording(recording));
                }
            }

            if (recordingStartDate >= startWeek && recordingStartDate < endWeek)
            {
                total.Week = total.Week.Add(GetTimeSpanOfRecording(recording));
                if (costCenterSet != null)
                {
                    costCenterSet.Week = costCenterSet.Week.Add(GetTimeSpanOfRecording(recording));
                }
            }

            if (recordingStartDate >= startMonth && recordingStartDate < endMonth)
            {
                total.Month = total.Month.Add(GetTimeSpanOfRecording(recording));
                if (costCenterSet != null)
                {
                    costCenterSet.Month = costCenterSet.Month.Add(GetTimeSpanOfRecording(recording));
                }
            }
        }

        foreach (var center in list.OrderBy(x => x.Title))
        {
            yield return center;
        }
            
        yield return total;
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
        _stundenMeisterPlugin.Data.CurrentTimeRecording = createdItem;
        return createdItem;
    }

    /// <summary>
    /// Starts a new recording of timing.
    /// Ends the current one and creates a new time record
    /// </summary>
    /// <param name="costCenter">Cost center whose timing will be started</param>
    public void StartNewRecording(IElement costCenter)
    {
        EndRecording();

        var logic = new TimeRecordingLogic(StundenMeisterPlugin.Get());

        var currentTimeRecording = logic.CreateAndAddNewTimeRecoding();
        currentTimeRecording.set(nameof(TimeRecording.startDate), DateTime.UtcNow);
        currentTimeRecording.set(nameof(TimeRecording.endDate), DateTime.UtcNow);
        currentTimeRecording.set(nameof(TimeRecording.isActive), true);
        currentTimeRecording.set(nameof(TimeRecording.costCenter), costCenter);
    }

    /// <summary>
    /// Ends the current active recording
    /// </summary>
    public void EndRecording()
    {
        var currentTimeRecording = _stundenMeisterPlugin.Data.CurrentTimeRecording;
            
        if (currentTimeRecording != null)
        {
            currentTimeRecording.set(nameof(TimeRecording.isActive), false);
            _stundenMeisterPlugin.Data.CurrentTimeRecording = null;
        }
            
        // By the way. Check for any other active time recording
        CheckForActiveTimeRecordingsAndDeactivate();
    }

    /// <summary>
    /// Changes the cost center and activates the timing, if there is currently an active
    /// timing
    /// </summary>
    /// <param name="selectedCostCenter">Costcenter to which the system switches</param>
    public void ChangeCostCenter(IElement selectedCostCenter)
    {
        var costCenter =
            _stundenMeisterPlugin.Data?.CurrentTimeRecording
                ?.getOrDefault<IElement>(nameof(TimeRecording.costCenter));

        if (costCenter != null && costCenter == selectedCostCenter)
        {
            return;
        }

        if (_stundenMeisterPlugin.Data?.CurrentTimeRecording?.getOrDefault<bool>(nameof(TimeRecording.isActive)) != true)
        {
            // If, there is no "isActive" time recording, then do not start a new time recording
            return;
        }

        var recordingLogic = new TimeRecordingLogic(_stundenMeisterPlugin);
        recordingLogic.StartNewRecording(selectedCostCenter);
    }

    /// <summary>
    /// Checks all time recordings and deactivate them if there are still some
    /// active time recordings
    /// </summary>
    private void CheckForActiveTimeRecordingsAndDeactivate()
    {
        foreach (var element in _stundenMeisterPlugin.Data.Extent.elements()
                     .WhenMetaClassIs(
                         _stundenMeisterPlugin.Data.ClassTimeRecording)
                     .WhenPropertyHasValue(
                         nameof(TimeRecording.isActive), true)
                     .OfType<IElement>())
        {
            element.set(nameof(TimeRecording.isActive), false);
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
            // Check for current end date
            var currentEndDate = currentTimeRecording.get<DateTime>(nameof(TimeRecording.endDate));

            // While current time recording is active, advance content
            var endDate = DateTime.UtcNow;

            if (hibernationActive && (endDate - currentEndDate) > hibernationTime)
            {
                // Ok, we have a hibernation overflow
                _stundenMeisterPlugin.Data.HibernationDetected = true;
            }
            else
            {
                // No hibernation, we can continue
                _stundenMeisterPlugin.Data.HibernationDetected = false;
                currentTimeRecording.set(nameof(TimeRecording.endDate), endDate);
            }
        }
    }

    /// <summary>
    /// If the current logic is in hibernation, then the use layer must confirm
    /// or reject the hibernation before the time estimation will continue
    /// </summary>
    /// <param name="continueRecording">true, if the hibernation is confirmed</param>
    public void ConfirmHibernation(bool continueRecording)
    {
        if (!_stundenMeisterPlugin.Data.HibernationDetected)
        {
            // No active hibernation
            return;
        }
            
        var currentTimeRecording = StundenMeisterData.TheOne.CurrentTimeRecording;
        if (currentTimeRecording == null)
        {
            // Nothing to do... no active recording
            return;
        }
            
        if (continueRecording)
        {
            // Ok, it is OK to continue with timing
            var endDate = DateTime.UtcNow;

            _stundenMeisterPlugin.Data.HibernationDetected = false;
            currentTimeRecording.set(nameof(TimeRecording.endDate), endDate);
        }
        else
        {
            // No, the session has ended.
            EndRecording();
        }
            
        _stundenMeisterPlugin.Data.HibernationDetected = false;
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

        foreach (var recording in _stundenMeisterPlugin.Data.Extent
                     .elements()
                     .WhenMetaClassIs(_stundenMeisterPlugin.Data.ClassTimeRecording)
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
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => new DateTime(day.Year, day.Month, day.Day).AddDays(-6),
            DayOfWeek.Monday => new DateTime(day.Year, day.Month, day.Day),
            DayOfWeek.Tuesday => new DateTime(day.Year, day.Month, day.Day).AddDays(-1),
            DayOfWeek.Wednesday => new DateTime(day.Year, day.Month, day.Day).AddDays(-2),
            DayOfWeek.Thursday => new DateTime(day.Year, day.Month, day.Day).AddDays(-3),
            DayOfWeek.Friday => new DateTime(day.Year, day.Month, day.Day).AddDays(-4),
            DayOfWeek.Saturday => new DateTime(day.Year, day.Month, day.Day).AddDays(-5),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public DateTime FindEndOfWeek(DateTime day)
    {
        var dayOfWeek = day.DayOfWeek;
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => new DateTime(day.Year, day.Month, day.Day).AddDays(1),
            DayOfWeek.Monday => new DateTime(day.Year, day.Month, day.Day).AddDays(7),
            DayOfWeek.Tuesday => new DateTime(day.Year, day.Month, day.Day).AddDays(6),
            DayOfWeek.Wednesday => new DateTime(day.Year, day.Month, day.Day).AddDays(5),
            DayOfWeek.Thursday => new DateTime(day.Year, day.Month, day.Day).AddDays(4),
            DayOfWeek.Friday => new DateTime(day.Year, day.Month, day.Day).AddDays(3),
            DayOfWeek.Saturday => new DateTime(day.Year, day.Month, day.Day).AddDays(2),
            _ => throw new ArgumentOutOfRangeException()
        };
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