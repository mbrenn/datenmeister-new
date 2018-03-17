using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.UserInteractions
{
    public class DefaultElementInteraction : IElementInteraction
    {
        private readonly Action _action;

        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the DefaultElementInteraction
        /// </summary>
        /// <param name="name"></param>
        public DefaultElementInteraction(string name, Action action)
        {
            _action = action;
            Name = name;
        }

        public void Action(IElement element, IObject parameters)
        {
            _action();
        }
    }
}