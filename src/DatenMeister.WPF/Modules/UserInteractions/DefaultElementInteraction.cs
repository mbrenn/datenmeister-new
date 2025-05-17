using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Modules.UserInteractions
{
    public class DefaultElementInteraction : IElementInteraction
    {
        private readonly Action<INavigationGuest, IObject> _action;

        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the DefaultElementInteraction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action">Action to be executed</param>
        public DefaultElementInteraction(string name, Action<INavigationGuest, IObject> action)
        {
            _action = action;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the DefaultElementInteraction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action">Action to be executed</param>
        public DefaultElementInteraction(string name, Action<IObject> action)
        {
            _action = (x,y) => action(y);
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the DefaultElementInteraction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action">Action to be executed</param>
        public DefaultElementInteraction(string name, Action action)
        {
            _action = (x,y) => action();
            Name = name;
        }

        public void Execute(INavigationGuest navigationGuest, IObject element, IObject? parameters)
        {
            _action(navigationGuest, element);
        }
    }
}