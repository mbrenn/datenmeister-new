using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Modules.DataViews.Evaluation
{
    public class FilterPropertyNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(FilterPropertyNodeEvaluation));
        public bool IsResponsible(IElement node)
        {
            
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.@equals(_DataViews.TheOne.__FilterPropertyNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FlattenNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = evaluation.GetElementsForViewNode(inputNode);

            var property = viewNode.getOrDefault<string>(_DataViews._FilterPropertyNode.property);
            if (property == null)
            {
                Logger.Warn("Property not found");
                return new PureReflectiveSequence();
            }

            var propertyValue = viewNode.getOrDefault<string>(_DataViews._FilterPropertyNode.value);
            if (propertyValue == null)
            {
                Logger.Warn("Property Value not found");
                return new PureReflectiveSequence();
            }

            var comparisonMode = viewNode.getOrNull<ComparisonMode>(_DataViews._FilterPropertyNode.comparisonMode);
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
        private IEnumerable<object> FilterElementsForPropertyNode(IReflectiveCollection input, string property, string propertyValue, ComparisonMode comparisonMode)
        {
            foreach (var element in input.OfType<IObject>())
            {
                if (!element.isSet(property))
                {
                    // Element is not set
                    continue;
                }

                var elementValue = element.getOrDefault<string>(property);

                bool isIn;
                switch (comparisonMode)
                {
                    case ComparisonMode.Equal:
                        isIn = elementValue == propertyValue;
                        break;
                    case ComparisonMode.NotEqual:
                        isIn = elementValue != propertyValue;
                        break;
                    case ComparisonMode.Contains:
                        isIn = elementValue.Contains(propertyValue);
                        break;
                    case ComparisonMode.DoesNotContain:
                        isIn = !elementValue.Contains(propertyValue);
                        break;
                    case ComparisonMode.GreaterThan:
                        isIn = string.Compare(elementValue, propertyValue, StringComparison.Ordinal) > 0;
                        break;
                    case ComparisonMode.GreaterOrEqualThan:
                        isIn = string.Compare(elementValue, propertyValue, StringComparison.Ordinal) >= 0;
                        break;
                    case ComparisonMode.LighterThan:
                        isIn = string.Compare(elementValue, propertyValue, StringComparison.Ordinal) < 0;
                        break;
                    case ComparisonMode.LighterOrEqualThan:
                        isIn = string.Compare(elementValue, propertyValue, StringComparison.Ordinal) <= 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(comparisonMode), comparisonMode, null);
                }

                if (isIn)
                {
                    yield return element;
                }
            }
        }
    }
}