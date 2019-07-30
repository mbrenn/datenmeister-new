using System;
using System.Security.RightsManagement;

namespace StundenMeister.Model
{
    public class TimeRecording
    {
        public DateTime startDate { get; set; }
        
        public DateTime endDate { get; set; }
        
        public bool isActive { get; set; }
    }
}