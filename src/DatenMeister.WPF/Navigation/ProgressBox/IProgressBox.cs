namespace DatenMeister.WPF.Navigation.ProgressBox
{
    /// <summary>
    /// The interface of the progressbox.
    /// The methods here within may 
    /// </summary>
    public interface IProgressBox
    {
        /// <summary>
        /// Sets the progress of the box. 
        /// </summary>
        /// <param name="percentage">Percentage which is currently achieved or -1, if indeterminate</param>
        /// <param name="text">Text to be shown</param>
        void SetProgress(double percentage, string text);

        /// <summary>
        /// Closes the navigation box
        /// </summary>
        void CloseProgress();
    }
}