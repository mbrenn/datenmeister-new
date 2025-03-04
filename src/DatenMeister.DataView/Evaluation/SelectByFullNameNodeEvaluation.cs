﻿using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;
using System;

namespace DatenMeister.DataView.Evaluation
{
    public class SelectByFullNameNodeEvaluation : IDataViewNodeEvaluation
    {
        private static readonly ILogger Logger = new ClassLogger(typeof(SelectByPathNodeEvaluation));

        public bool IsResponsible(IElement node)
        {
            var metaClass = node.getMetaClass();
            return metaClass != null &&
                   metaClass.equals(_DatenMeister.TheOne.DataViews.__SelectByFullNameNode);
        }

        public IReflectiveCollection Evaluate(DataViewEvaluation evaluation, IElement viewNode)
        {
            var inputNode = viewNode.getOrDefault<IElement>(_DatenMeister._DataViews._SelectByFullNameNode.input);
            if (inputNode == null)
            {
                Logger.Warn("Input node not found");
                throw new InvalidOperationException("Input node not found");
            }

            var input = evaluation.GetElementsForViewNode(inputNode);

            var pathNode = viewNode.getOrDefault<string>(_DatenMeister._DataViews._SelectByFullNameNode.path);
            if (pathNode == null)
            {
                Logger.Warn("Path is not set");
                throw new InvalidOperationException("Path is not set");
            }

            var targetElement = NamedElementMethods.GetByFullName(input, pathNode);
            if (targetElement == null)
            {
                // Path is not found
                throw new InvalidOperationException($"Path is not found: {pathNode}");
            }

            return new TemporaryReflectiveSequence(NamedElementMethods.GetAllPropertyValues(targetElement));
        }
    }
}