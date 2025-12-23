using System.Collections;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;

namespace DatenMeister.DataView;

/// <summary>
/// Implements a factory for data view extents
/// </summary>
public class DataViewExtentFactory : IEnumerable<IExtent>
{
    /// <summary>
    /// Stores the logic for the data view
    /// </summary>
    private readonly DataViewLogic _dataViewLogic;

    /// <summary>
    /// Stores the scope storage
    /// </summary>
    private readonly IScopeStorage _scopeStorage;

    /// <summary>
    /// Initializes a new instance of the <see cref="DataViewExtentFactory"/> class
    /// </summary>
    /// <param name="dataViewLogic">The logic for the dataviews</param>
    /// <param name="scopeStorage">The scope storage being associated</param>
    public DataViewExtentFactory(DataViewLogic dataViewLogic, IScopeStorage scopeStorage)
    {
        _dataViewLogic = dataViewLogic;
        _scopeStorage = scopeStorage;
    }

    /// <inheritdoc />
    public IEnumerator<IExtent> GetEnumerator()
    {
        foreach (var dataView in _dataViewLogic.GetDataViewElements())
        {
            yield return new DataViewExtent(dataView, _dataViewLogic, _scopeStorage);
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();
}