using System.Windows.Controls.Ribbon;

namespace DatenMeister.WPF.Windows
{
    public interface IHasRibbon
    {
        /// <summary>
        /// Gets the ribbon
        /// </summary>
        /// <returns></returns>
        Ribbon GetRibbon();
    }
}