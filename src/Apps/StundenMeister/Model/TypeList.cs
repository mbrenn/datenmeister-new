using System;
using System.Runtime.Remoting.Messaging;
using System.Windows.Documents;

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