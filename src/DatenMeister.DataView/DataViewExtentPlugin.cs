using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.DataView
{
    public class DataViewExtentPlugin : IEnumerable<IExtent>
    {
        private readonly DataViewLogic _dataViewLogic;

        /// <summary>
        /// Initializes a new instance of the workspace logic
        /// </summary>
        /// <param name="dataViewLogic">The logic for the dataviews</param>
        public DataViewExtentPlugin(DataViewLogic dataViewLogic)
        {
            _dataViewLogic = dataViewLogic;
        }

        public IEnumerator<IExtent> GetEnumerator()
        {
            foreach (var dataView in _dataViewLogic.GetDataViewElements())
            {
                yield return new DataViewExtent(dataView, _dataViewLogic);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}