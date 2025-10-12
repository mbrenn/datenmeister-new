using System.Text.RegularExpressions;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

public class RowFilterByPropertyValueNodeEvaluation : IDataViewNodeEvaluation
{
    private static readonly ILogger Logger = new ClassLogger(typeof(RowFilterByPropertyValueNodeEvaluation));

    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Row.__RowFilterByPropertyValueNode);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Row._RowFilterByPropertyValueNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        var input = evaluation.GetElementsForViewNode(inputNode);

        var property = viewNode.getOrDefault<string>(_DataViews._Row._RowFilterByPropertyValueNode.property);
        if (property == null)
        {
            Logger.Warn("Property not found");
            return new PureReflectiveSequence();
        }

        var propertyValue = viewNode.getOrDefault<string>(_DataViews._Row._RowFilterByPropertyValueNode.value);
        if (propertyValue == null)
        {
            Logger.Warn("Property Value not found");
            return new PureReflectiveSequence();
        }

        var comparisonMode =
            viewNode.getOrNull<_DataViews.___ComparisonMode>(
                _DataViews._Row._RowFilterByPropertyValueNode.comparisonMode);
        if (comparisonMode == null)
        {
            Logger.Warn("Comparison not found");
            return new PureReflectiveSequence();
        }

        return new TemporaryReflectiveSequence(FilterElementsForPropertyNode(input, property, propertyValue,
            comparisonMode.Value));
    }


    /// <summary>
    /// Filters the elements of the reflective sequence according the rules
    /// </summary>
    /// <param name="input">Elements to be filtered</param>
    /// <param name="property">Property upon which the filtering shall be done</param>
    /// <param name="propertyValue">Value of the property that will be used as filtering value</param>
    /// <param name="comparisonMode">The type of the comparison</param>
    /// <returns>Enumeration of elements being in the filter</returns>
    private IEnumerable<object> FilterElementsForPropertyNode(IReflectiveCollection input, string property,
        string propertyValue, _DataViews.___ComparisonMode comparisonMode)
    {
        Regex? regex = null;
        if (comparisonMode == _DataViews.___ComparisonMode.RegexMatch
            || comparisonMode == _DataViews.___ComparisonMode.RegexNoMatch)
        {
            regex = new Regex(propertyValue);
        }
        
        foreach (var element in input.OfType<IObject>())
        {
            if (!element.isSet(property))
            {
                // Element is not set
                continue;
            }

            var elementValue = element.getOrDefault<string>(property);

            var isIn = comparisonMode switch
            {
                _DataViews.___ComparisonMode.Equal => elementValue == propertyValue,
                _DataViews.___ComparisonMode.NotEqual => elementValue != propertyValue,
                _DataViews.___ComparisonMode.Contains => elementValue.Contains(propertyValue),
                _DataViews.___ComparisonMode.DoesNotContain => !elementValue.Contains(propertyValue),
                _DataViews.___ComparisonMode.GreaterThan =>
                    string.Compare(elementValue, propertyValue, StringComparison.Ordinal) > 0,
                _DataViews.___ComparisonMode.GreaterOrEqualThan => string.Compare(elementValue,
                    propertyValue,
                    StringComparison.Ordinal) >= 0,
                _DataViews.___ComparisonMode.LighterThan =>
                    string.Compare(elementValue, propertyValue, StringComparison.Ordinal) < 0,
                _DataViews.___ComparisonMode.LighterOrEqualThan => string.Compare(elementValue,
                    propertyValue,
                    StringComparison.Ordinal) <= 0,
                _DataViews.___ComparisonMode.RegexMatch => regex?.IsMatch(elementValue) ?? false,
                _DataViews.___ComparisonMode.RegexNoMatch => regex?.IsMatch(elementValue) == false,
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonMode), comparisonMode, null)
            };

            if (isIn)
            {
                yield return element;
            }
        }
    }
}