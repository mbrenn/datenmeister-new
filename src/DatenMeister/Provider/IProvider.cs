using System.Collections.Generic;

namespace DatenMeister.Provider
{
    public interface IProvider
    {
        IProviderObject CreateElement(string metaClassUri);

        void DeleteElement(string id);

        IProviderObject Get(string id);

        Dictionary<string, object> GetRootObjects();

        Dictionary<string, object> GetAllObjects();
    }
}