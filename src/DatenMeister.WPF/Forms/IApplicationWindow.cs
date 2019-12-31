#nullable enable

namespace DatenMeister.WPF.Forms
{
    /// <summary>
    /// This interface can be used by the main application window
    /// This interface is used by the view extensions to trigger certain
    /// menus only for the main application
    /// </summary>
    public interface IApplicationWindow
    {
        /// <summary>
        /// Gets or sets a value whether the closing shall occur without
        /// User acknowledgement. This flag is used when another function requests the closing
        /// of the mainwindow while the user already has acknowledged the closing
        /// </summary>
        bool DoCloseWithoutAcknowledgement { get; set; }
    } 
}