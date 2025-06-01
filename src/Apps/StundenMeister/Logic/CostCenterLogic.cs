namespace StundenMeister.Logic;

public class CostCenterLogic(StundenMeisterPlugin plugin)
{
    /// <summary>
    /// Gets the cost centers which are in the database
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IElement> GetCostCenters()
    {
        return plugin.Data.Extent
            .elements()
            .WhenMetaClassIs(plugin.Data.ClassCostCenter)
            .OfType<IElement>();
    }

    /// <summary>
    /// Performs the notification if there is a change in the cost centers
    /// </summary>
    /// <param name="action">Action to be called</param>
    public void NotifyForCostCenterChange(Action<IExtent, IObject> action)
    {
        plugin.EventManager.RegisterFor(
            plugin.Data.Extent,
            (x,y) =>
            {
                if ((y as IElement)?.getMetaClass()?.equals(plugin.Data.ClassCostCenter) == true)
                {
                    action(x, y);
                }
            });
    }
}