using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Proxies;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewEvaluation
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DataViewEvaluation));

        private readonly IWorkspaceLogic _workspaceLogic;

        private int _referenceCount;

        private const int MaximumReferenceCount = 1000;

        public DataViewEvaluation(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        public IReflectiveSequence GetElementsForViewNode(IElement viewNode)
        {
            if (viewNode == null)
            {
                throw new ArgumentException(nameof(viewNode));
            }

            // Check, if viewnode has been visited
            _referenceCount++;
            if (_referenceCount > MaximumReferenceCount)
            {
                Logger.Warn("Maximum number of references are evaluated in dataview evaluation");
                return new PureReflectiveSequence();
            }


            var dataview = _workspaceLogic.GetTypesWorkspace().Create<FillTheDataViews, _DataViews>();
            var metaClass = viewNode.getMetaClass();
            if (metaClass == null)
            {
                Logger.Warn($"Unknown type of viewnode: null");
                return new PureReflectiveSequence();
            }

            if (metaClass.equals(dataview.__SourceExtentNode))
                return GetElementsForSourceExtent(viewNode);

            if (metaClass.equals(dataview.__SelectPathNode))
                return GetElementsForPathNode(viewNode);

            if (metaClass.equals(dataview.__FlattenNode))
                return GetElementsForFlattenNode(viewNode);

            if (metaClass.equals(dataview.__FilterTypeNode))
                return GetElementsForFilterTypeNode(viewNode);

            if (metaClass.equals(dataview.__FilterPropertyNode))
                return GetElementsForFilterPropertyNode(viewNode);

            Logger.Warn($"Unknown type of viewnode: {viewNode.getMetaClass()}");

            return new PureReflectiveSequence();
        }

        private IReflectiveSequence GetElementsForSourceExtent(IElement viewNode)
        {
            var workspaceName = viewNode.getOrDefault<string>(_DataViews._SourceExtentNode.workspace);
            if (string.IsNullOrEmpty(workspaceName))
            {
                workspaceName = WorkspaceNames.NameData;
            }

            var extentUri = viewNode.getOrDefault<string>(_DataViews._SourceExtentNode.extentUri);
            var workspace = _workspaceLogic.GetWorkspace(workspaceName);
            if (workspace == null)
            {
                Logger.Warn($"Workspace is not found: {workspaceName}");
                return new PureReflectiveSequence();
            }

            var extent = workspace.FindExtent(extentUri);
            if (extent == null)
            {
                Logger.Warn($"Extent is not found: {extentUri}");
                return new PureReflectiveSequence();
            }

            return extent.elements();
        }

        private IReflectiveSequence GetElementsForPathNode(IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._SelectPathNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = GetElementsForViewNode(inputNode);

            var pathNode = viewNode.getOrDefault<string>(_DataViews._SelectPathNode.path);
            if (pathNode == null)
            {
                Logger.Warn($"Path is not set");
                return new PureReflectiveSequence();
            }

            var targetElement = NamedElementMethods.GetByFullName(input, pathNode);
            if (targetElement == null)
            {
                // Path is not found
                return new PureReflectiveSequence();
            }
            
            return new TemporaryReflectiveSequence(NamedElementMethods.GetAllPropertyValues(targetElement));
        }

        private IReflectiveSequence GetElementsForFlattenNode(IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FlattenNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            return GetElementsForViewNode(inputNode).GetAllDescendants();
        }

        private IReflectiveSequence GetElementsForFilterTypeNode(IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FilterTypeNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = GetElementsForViewNode(inputNode);

            var type = viewNode.getOrDefault<IElement>(_DataViews._FilterTypeNode.type);
            if (type == null)
            {
                Logger.Warn("Type is not given");
                return new PureReflectiveSequence();
            }

            return new TemporaryReflectiveSequence(input.WhenMetaClassIs(type));
        }

        private IReflectiveSequence GetElementsForFilterPropertyNode(IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DataViews._FlattenNode.input);
            if (inputNode == null)
            {
                Logger.Warn($"Input node not found");
                return new PureReflectiveSequence();
            }

            var input = GetElementsForViewNode(inputNode);

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
        private IEnumerable<object> FilterElementsForPropertyNode(IReflectiveSequence input, string property, string propertyValue, ComparisonMode comparisonMode)
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