﻿using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DataView.Evaluation;

public class NodeReferenceNodeEvaluation : IDataViewNodeEvaluation
{
    public bool IsResponsible(IElement node)
    {
        var metaClass = node.getMetaClass();
        return metaClass != null &&
               metaClass.equals(_DataViews.TheOne.Node.__ReferenceViewNode);
    }

    public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
    {
        if(evaluation.WorkspaceLogic == null)
            throw new InvalidOperationException("WorkspaceLogic is null");
        
        var workspaceId = viewNode.getOrDefault<string>(
            _DataViews._Node._ReferenceViewNode.workspaceId);
        var itemUri = viewNode.getOrDefault<string>(
            _DataViews._Node._ReferenceViewNode.itemUri);

        var foundObject = evaluation.WorkspaceLogic.FindObject(workspaceId, itemUri) as IElement;
        if (foundObject == null)
        {
            throw new InvalidOperationException("The item has not been found");
        }        
        
        // Check, if the viewnode is a query statement, if that is the case, get the content of the resultNode
        if (foundObject.getMetaClass()?.equals(_DataViews.TheOne.__QueryStatement) == true)
        {
            foundObject = foundObject.getOrDefault<IElement>(_DataViews._QueryStatement.resultNode)
                ?? throw new InvalidOperationException("resultNode is not set");
        }
        
        return evaluation.GetElementsForViewNode(foundObject);
    }
}