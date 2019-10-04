using System;
using System.Security.RightsManagement;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace StundenMeister.Model
{
    public class TimeRecording
    {
        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

        public bool isActive { get; set; }

        public CostCenter costCenter { get; set; }
    }
}