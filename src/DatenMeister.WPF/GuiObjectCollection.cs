using DatenMeister.WPF.Forms.Base.ViewExtensions;
using System.Collections.Generic;

namespace DatenMeister.WPF
{
    /// <summary>
    /// The class contains all root objects which are required to provide the necessary information about
    /// the Gui
    /// </summary>
    public class GuiObjectCollection
    {
        /// <summary>
        /// Defines the synchronization object
        /// </summary>
        private static readonly object SyncObject = new object();

        /// <summary>
        /// Stores the singleton
        /// </summary>
        private static GuiObjectCollection _theOne;

        /// <summary>
        /// Gets the one and only Gui Object Collection instance
        /// </summary>
        public static GuiObjectCollection TheOne
        {
            get
            {
                if (_theOne == null)
                {
                    lock (SyncObject)
                    {
                        if (_theOne == null)
                        {
                            _theOne = new GuiObjectCollection();
                        }
                    }
                }

                return _theOne;
            }
        }

        /// <summary>
        /// Gets a list of the allowed view extension factories
        /// </summary>
        public List<IViewExtensionFactory> ViewExtensionFactories { get; } = new List<IViewExtensionFactory>();
    }
}