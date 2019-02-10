using System.Windows.Media;

namespace DatenMeister.WPF.Modules
{
    /// <summary>
    /// Single source to retrieve the icons for the project
    /// </summary>
    public interface IIconRepository
    {
        /// <summary>
        /// Gets the icon by name
        /// </summary>
        /// <param name="name">The name of the icon</param>
        /// <returns>The image behind the filename or null, if the icon is not available
        /// in the repository</returns>
        ImageSource GetIcon(string name);
    }
}
