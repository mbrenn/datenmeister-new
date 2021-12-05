using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Json
{
    public class MofJsonDeconverter
    {        
        public IObject ConvertToObject(MofObjectAsJson jsonObject)
        {   
            var result = InMemoryObject.CreateEmpty();

            foreach (var pair in jsonObject.v)
            {
                result.set(pair.Key, DirectJsonDeconverter.ConvertJsonValue(pair.Value));
            }

            return result;
        }
    }
}