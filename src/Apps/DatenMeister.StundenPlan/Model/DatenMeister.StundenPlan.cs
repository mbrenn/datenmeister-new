#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.StundenPlan.Model
{
    public class _StundenPlan
    {
        public class _WeeklyPeriodicEvent
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @timeStart = "timeStart";
            public IElement? @_timeStart = null;

            public static string @hoursDuration = "hoursDuration";
            public IElement? @_hoursDuration = null;

            public static string @weekInterval = "weekInterval";
            public IElement? @_weekInterval = null;

            public static string @weekOffset = "weekOffset";
            public IElement? @_weekOffset = null;

            public static string @onMonday = "onMonday";
            public IElement? @_onMonday = null;

            public static string @onTuesday = "onTuesday";
            public IElement? @_onTuesday = null;

            public static string @onWednesday = "onWednesday";
            public IElement? @_onWednesday = null;

            public static string @onThursday = "onThursday";
            public IElement? @_onThursday = null;

            public static string @onFriday = "onFriday";
            public IElement? @_onFriday = null;

            public static string @onSaturday = "onSaturday";
            public IElement? @_onSaturday = null;

            public static string @onSunday = "onSunday";
            public IElement? @_onSunday = null;

        }

        public _WeeklyPeriodicEvent @WeeklyPeriodicEvent = new _WeeklyPeriodicEvent();
        public MofObjectShadow @__WeeklyPeriodicEvent = new MofObjectShadow("dm:///types.stundenplan.datenmeister/#954ad359-1a92-4661-bc58-83ea4517d493");

        public static readonly _StundenPlan TheOne = new _StundenPlan();

    }

}
