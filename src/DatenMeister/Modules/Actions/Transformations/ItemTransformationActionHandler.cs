﻿using System;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Actions.ActionHandler;
using DatenMeister.Provider.DynamicRuntime;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Actions.Transformations
{
    /// <summary>
    /// Defines the transformation class for the item transformation
    /// </summary>
    public class ItemTransformationActionHandler : IActionHandler
    {
        private static readonly ILogger logger = new ClassLogger(typeof(ItemTransformationActionHandler));
        public bool IsResponsible(IElement node)
        {
            return node.getMetaClass()?.@equals(
                _DatenMeister.TheOne.Actions.__TransformItemsAction) == true;
        }

        public void Evaluate(ActionLogic actionLogic, IElement action)
        {
            var metaClass = action.getOrDefault<IElement>(_DatenMeister._Actions._TransformItemsAction.metaClass);
            var runtimeClass = action.getOrDefault<string>(_DatenMeister._Actions._TransformItemsAction.runtimeClass);
            var workspace =
                action.getOrDefault<string>(_DatenMeister._Actions._TransformItemsAction.workspace);
            var path = action.getOrDefault<string>(_DatenMeister._Actions._TransformItemsAction.path);
            
            var sourceWorkspace = actionLogic.WorkspaceLogic.GetWorkspace(workspace);
            if (sourceWorkspace == null)
            {
                var message = $"sourceWorkspace is not found {workspace}";
                logger.Error(message);

                throw new InvalidOperationException(message);
            }
            
            var sourceElement = sourceWorkspace.Resolve(path, ResolveType.NoMetaWorkspaces);

            if (sourceElement == null)
            {
                var message = $"sourcePath is not found ${path}";
                logger.Error(message);

                throw new InvalidOperationException(message);
            }

            // Depending on type, get the reflective instance
            var targetCollection = CopyElementsActionHandler.GetCollectionFromResolvedElement(sourceElement)
                                   ?? throw new InvalidOperationException(
                                       "targetElement is null");

            var transformer =
                DynamicRuntimeProviderLoader.CreateInstanceByRuntimeClass<IItemTransformation>(runtimeClass);

            foreach (var item in targetCollection.GetAllCompositesIncludingThemselves())
            {
                if (!(item is IElement asElement)) continue;
                if (metaClass != null)
                {
                    if (asElement.metaclass?.Equals(metaClass) != true)
                    {
                        continue;
                    }
                }
                
                // Transform item
                transformer.TransformItem(asElement);
            }
        }
    }
}