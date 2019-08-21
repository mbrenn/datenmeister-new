using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace StundenMeister.Logic
{
    public class TimeRecordingSet
    {
        public string Title { get; set; }
        
        public TimeSpan Day { get; set; }
        
        public TimeSpan Week { get; set; }
        
        public TimeSpan Month { get; set; }
        
        public IElement CostCenter { get; set; }
    }
}