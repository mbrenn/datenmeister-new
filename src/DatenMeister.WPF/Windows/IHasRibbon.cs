using System.Windows.Controls.Ribbon;

namespace DatenMeisterWPF.Windows
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