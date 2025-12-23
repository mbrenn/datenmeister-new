using BurnSystems.Logging;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView;

/// <summary>
/// Provides helper methods for the evaluation of data view nodes
/// </summary>
public static class DataViewNodeEvaluationHelper
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(DataViewNodeEvaluationHelper));
    
    /// <summary>
    /// Gets the input node for the given view node
    /// </summary>
    /// <param name="viewNodeEvaluation">The evaluation engine being used</param>
    /// <param name="viewNode">The view node to be evaluated</param>
    /// <returns>The resulting collection</returns>
    public static IReflectiveCollection GetInputNode(this DataViewEvaluation viewNodeEvaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement?>(_DataViews._Column._ColumnFilterIncludeOnlyNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        return viewNodeEvaluation.GetElementsForViewNode(inputNode);
    }
}