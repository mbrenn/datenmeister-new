using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;

namespace StundenMeister.Logic
{
    public class CostCenterLogic
    {
        private readonly StundenMeisterLogic _logic;

        public CostCenterLogic(StundenMeisterLogic logic)
        {
            _logic = logic;
        }

        /// <summary>
        /// Gets the cost centers which are in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IElement> GetCostCenters()
        {
            return _logic.Data.Extent
                .elements()
                .WhenMetaClassIs(_logic.Data.ClassCostCenter)
                .OfType<IElement>();
        }

        /// <summary>
        /// Performs the notification if there is a change in the cost centers
        /// </summary>
        /// <param name="action">Action to be called</param>
        public void NotifyForCostCenterChange(Action<IExtent, IObject> action)
        {
            _logic.EventManager.RegisterFor(
                _logic.Data.Extent,
                (x,y) =>
                {
                    if ((y as IElement)?.getMetaClass()?.@equals(_logic.Data.ClassCostCenter) == true)
                    {
                        action(x, y);
                    }
                });
        }
    }
}