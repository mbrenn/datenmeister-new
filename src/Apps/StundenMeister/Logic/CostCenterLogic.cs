namespace StundenMeister.Logic;

public class CostCenterLogic
{
    private readonly StundenMeisterPlugin _plugin;

    public CostCenterLogic(StundenMeisterPlugin plugin)
    {
        _plugin = plugin;
    }

    /// <summary>
    /// Gets the cost centers which are in the database
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IElement> GetCostCenters()
    {
        return _plugin.Data.Extent
            .elements()
            .WhenMetaClassIs(_plugin.Data.ClassCostCenter)
            .OfType<IElement>();
    }

    /// <summary>
    /// Performs the notification if there is a change in the cost centers
    /// </summary>
    /// <param name="action">Action to be called</param>
    public void NotifyForCostCenterChange(Action<IExtent, IObject> action)
    {
        _plugin.EventManager.RegisterFor(
            _plugin.Data.Extent,
            (x,y) =>
            {
                if ((y as IElement)?.getMetaClass()?.equals(_plugin.Data.ClassCostCenter) == true)
                {
                    action(x, y);
                }
            });
    }
}