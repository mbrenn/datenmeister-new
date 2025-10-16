using BurnSystems.Logging;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView;

public static class DataViewNodeEvaluationHelper
{
    private static readonly ILogger Logger = new ClassLogger(typeof(DataViewNodeEvaluationHelper));
    
    public static IReflectiveCollection GetInputNode(this DataViewEvaluation viewNodeEvaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Column._ColumnFilterIncludeOnlyNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        return viewNodeEvaluation.GetElementsForViewNode(inputNode);
    }
}