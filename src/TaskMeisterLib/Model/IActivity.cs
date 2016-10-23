using System;

namespace TaskMeister.Model
{
    public interface IActivity
    {
        string Name { get; set; }

        string Description { get; set; }

        ActivityState State { get; set; }

        DateTime Created { get; set; }

        DateTime StartDate { get; set; }

        DateTime FinishDate { get; set; }

        TimeSpan EstmatedDuration { get; set; }

    }
}