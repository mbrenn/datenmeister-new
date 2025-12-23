using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.DataView.Evaluation;

/// <summary>
/// Implements the evaluation for the row filter by metaclass node
/// </summary>
public class RowFilterByMetaclassNodeEvaluation : IDataViewNodeEvaluation
{
    /// <summary>
    /// Stores the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(RowFilterByMetaclassNodeEvaluation));

    /// <inheritdoc />
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Row.__RowFilterByMetaclassNode);
    }

    /// <inheritdoc />
    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var inputNode = viewNode.getOrDefault<IElement>(_DataViews._Row._RowFilterByMetaclassNode.input);
        if (inputNode == null)
        {
            Logger.Warn("Input node not found");
            return new PureReflectiveSequence();
        }

        var input = evaluation.GetElementsForViewNode(inputNode);

        var type = viewNode.getOrDefault<IElement>(_DataViews._Row._RowFilterByMetaclassNode.metaClass);
        var includeInherits = viewNode.getOrDefault<bool>(_DataViews._Row._RowFilterByMetaclassNode.includeInherits);
        if (type == null)
        {
            return new TemporaryReflectiveSequence(input.WhenMetaClassIsNotSet());
        }

        if (includeInherits)
        {
            return new TemporaryReflectiveSequence(
                input.WhenMetaClassIsOrSpecialized(type));
        }
        else
        {
            return new TemporaryReflectiveSequence(
                input.WhenMetaClassIs(type));    
        }
    }
}