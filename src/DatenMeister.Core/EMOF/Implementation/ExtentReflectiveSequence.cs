using System.Collections;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;

namespace DatenMeister.Core.EMOF.Implementation;

/// <summary>
/// Implements the reflective sequence for
/// </summary>
public class ExtentReflectiveSequence : IReflectiveSequence, IHasExtent
{
    /// <summary>
    /// Stores the extent which is abstracted by this class instance
    /// </summary>
    private readonly MofExtent _extent;

    /// <inheritdoc />
    public IExtent Extent => _extent;

    /// <summary>
    /// Initializes a new instance of the ExtentReflectiveSequence class
    /// </summary>
    /// <param name="extent">Extent to be covered by this reflective sequence</param>
    public ExtentReflectiveSequence(MofExtent extent)
    {
        _extent = extent;
    }

    /// <inheritdoc />
    public IEnumerator<object> GetEnumerator()
    {
        foreach (var element in _extent.Provider.GetRootObjects())
        {
            var resultElement = new MofElement(element, _extent)
            {
                // Sets also the directly associated extent
                Extent = _extent
            };

            yield return resultElement;
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    /// <inheritdoc />
    public bool add(object value)
        => AddInternal(value, -1);

    /// <inheritdoc />
    public bool addAll(IReflectiveSequence value)
    {
        bool? result = null;

        foreach (var element in value)
        {
            if (element == null) continue;

            if (result == null)
            {
                result = add(element);
            }
            else
            {
                result |= add(element);
            }
        }

        return result == true;
    }

    /// <inheritdoc />
    public void clear()
    {
        _extent.Provider.DeleteAllElements();
        _extent?.ChangeEventManager?.SendChangeEvent(_extent);
    }

    /// <inheritdoc />
    public bool remove(object? value)
    {
        if (value is MofObject valueAsObject)
        {
            var id = valueAsObject.ProviderObject.Id;
            if (id != null)
            {
                var result = _extent.Provider.DeleteElement(id);

                // Remove the extent allocation
                if (_extent == valueAsObject.Extent)
                {
                    valueAsObject.Extent = null;
                }

                UpdateContent();
                return result;
            }

            return false;
        }

        if (value is MofObjectShadow)
        {
            //_extent.Provider.DeleteElement(shadow);
        }

        throw new NotImplementedException("Only the deletion of MofObjects are supported: "
                                          + (value?.GetType().ToString() ?? "null"));
    }

    /// <inheritdoc />
    public int size()
        => _extent.Provider.GetRootObjects().Count();

    /// <inheritdoc />
    public void add(int index, object value)
    {
        AddInternal(value, index);
    }

    /// <summary>
    /// Adds the object internally
    /// </summary>
    /// <param name="value">Value to be added</param>
    /// <param name="index">Index of the object to be added</param>
    /// <returns>true, if object could be added</returns>
    private bool AddInternal(object value, int index)
    {
        if (value is MofObject valueAsObject)
        {
            if (valueAsObject.Extent == _extent || valueAsObject.Extent == null)
            {
                _extent.Provider.AddElement(valueAsObject.ProviderObject, index);
                valueAsObject.Extent = _extent;

                _extent?.ChangeEventManager?.SendChangeEvent(valueAsObject);
                UpdateContent();
                return true;
            }

            throw new NotImplementedException("Only objects from the extent are currently supported");
        }

        if (DotNetHelper.IsOfPrimitiveType(value))
        {
            throw new InvalidOperationException(
                $"An instance of a primitive type may not be added to the extent root elements: {value}");
        }

        if (_extent.ConvertForSetting(value) is IProviderObject convertedElement)
        {
            _extent?.Provider.AddElement(convertedElement, index);
        }

        UpdateContent();

        return true;
    }

    /// <inheritdoc />
    public object get(int index)
        =>
            new MofElement(
                _extent.Provider.GetRootObjects().ElementAt(index),
                _extent);

    /// <inheritdoc />
    public void remove(int index)
    {
        remove(
            new MofObject(
                _extent.Provider.GetRootObjects().ElementAt(index),
                _extent));
        UpdateContent();
    }

    /// <inheritdoc />
    public object set(int index, object value)
    {
        var size = this.size();
        if (index < 0 || index >= size)
        {
            throw new ArgumentException("Object could not be added due to wrong index: " + index);
        }

        var result = get(index);
        (value as MofObject)?.ProviderObject.SetContainer(null);
            
        remove(index);
        add(index, value);

        UpdateContent();
            

        return result;
    }

    /// <summary>
    /// Updates the content
    /// </summary>
    protected void UpdateContent()
    {
        _extent.ChangeEventManager?.SendChangeEvent(_extent);
        _extent.SignalUpdateOfContent();

        (_extent as MofUriExtent)?.ClearResolveCache();
    }
}