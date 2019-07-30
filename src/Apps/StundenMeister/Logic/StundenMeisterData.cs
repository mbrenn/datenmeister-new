using System.Dynamic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace StundenMeister.Logic
{
    public class StundenMeisterData
    {
        public static StundenMeisterData TheOne { get; } = new StundenMeisterData();
        
        public IElement ClassCostCenter { get; set; }
        
        public IElement ClassTimeRecording { get; set; }
        
        public IUriExtent Data { get; set; }
    }
}