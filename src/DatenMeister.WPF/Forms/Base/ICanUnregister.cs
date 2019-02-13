namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// This interface needs to be implemented by all classes that contain elements that need to be unregistered
    /// after changing the view
    /// </summary>
    public interface ICanUnregister
    {
        /// <summary>
        /// Requests the unregistering
        /// </summary>
        void Unregister();
    }
}