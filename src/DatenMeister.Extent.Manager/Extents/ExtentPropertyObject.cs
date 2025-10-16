using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Extent.Manager.Extents;

/// <summary>
/// Defines a MofObject wrapper which returns all the properties being allocated to the extent
/// within the interfaces of IObject and IObjectAllProperties
/// </summary>
public class ExtentPropertyObject(IExtent extent) : IObject, IObjectAllProperties
{
    private readonly IExtent _extent = extent ?? throw new ArgumentNullException(nameof(extent));

    public bool equals(object? other) => other is IExtent extent && _extent.equals(extent);

    public object? get(string property) => _extent.get(property);

    public void set(string property, object? value) => _extent.set(property, value);

    public bool isSet(string property) => _extent.isSet(property);

    public void unset(string property) => _extent.unset(property);
        
    public IEnumerable<string> getPropertiesBeingSet()
    {
        return (_extent as IObjectAllProperties)?.getPropertiesBeingSet() ?? new string[] { };
    }
}