using System.Collections;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;

namespace DatenMeister.DataView;

public class DataViewExtentFactory : IEnumerable<IExtent>
{
    private readonly DataViewLogic _dataViewLogic;
        
    private readonly IScopeStorage _scopeStorage;

    /// <summary>
    /// Initializes a new instance of the workspace logic
    /// </summary>
    /// <param name="dataViewLogic">The logic for the dataviews</param>
    /// <param name="scopeStorage">The scope storage being associated</param>
    public DataViewExtentFactory(DataViewLogic dataViewLogic, IScopeStorage scopeStorage)
    {
        _dataViewLogic = dataViewLogic;
        _scopeStorage = scopeStorage;
    }

    public IEnumerator<IExtent> GetEnumerator()
    {
        foreach (var dataView in _dataViewLogic.GetDataViewElements())
        {
            yield return new DataViewExtent(dataView, _dataViewLogic, _scopeStorage);
        }
    }

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}