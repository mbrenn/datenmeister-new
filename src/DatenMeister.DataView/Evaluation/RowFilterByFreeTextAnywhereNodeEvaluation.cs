using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models;

namespace DatenMeister.DataView.Evaluation;

public class RowFilterByFreeTextAnywhereNodeEvaluation : IDataViewNodeEvaluation
{
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.__RowFilterByFreeTextAnywhere);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        var input = evaluation.GetInputNode(viewNode);
        
        var wrappedViewNode = new DataViews.RowFilterByFreeTextAnywhere_Wrapper(viewNode);
        var freeText = wrappedViewNode.freeText;

        if (string.IsNullOrEmpty(freeText))
        {
            return input;       
        }

        return new RowFilterOnAnyProperty(input)
        {
            FreeText = freeText
        };
    }
}