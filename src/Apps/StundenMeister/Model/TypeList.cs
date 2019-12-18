using System;

namespace StundenMeister.Model
{
    public static class TypeList
    {
        public static Type[] Types =>
            new[]
            {
                typeof(CostCenter),
                typeof(TimeRecording)
            };
    }
}