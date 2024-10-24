﻿using System.Threading.Tasks;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Actions.ActionHandler
{
    /// <summary>
    /// Defines the interface for the actionhandler
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// Checks whether the given node is responsible for the action handler factory
        /// </summary>
        /// <param name="node">Node to be evaluated</param>
        /// <returns>true, if the factory is responsible</returns>
        public bool IsResponsible(IElement node);

        /// <summary>
        /// Evaluates the action
        /// </summary>
        /// <param name="actionLogic">Actionplugin to be handled</param>
        /// <param name="action">Action to be handled</param>
        public Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action);
    }
}