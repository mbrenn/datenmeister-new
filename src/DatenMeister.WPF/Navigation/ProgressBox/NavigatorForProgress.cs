using System;
using System.Windows;

#nullable enable

namespace DatenMeister.WPF.Navigation.ProgressBox
{
    /// <summary>
    /// Stores some methods which allows the showing of progress
    /// </summary>
    public class NavigatorForProgress
    {
        /// <summary>
        /// Creates a new progress box for the given navigation host
        /// </summary>
        /// <param name="navigationHost">Navigation host to be used</param>
        /// <returns>The created progress box</returns>
        public static IProgressBox CreateProgressBox(INavigationHost navigationHost)
        {
            if (navigationHost is Window window)
            {
                var newWindow = new WpfProgressBox
                {
                    Owner = window
                };
                newWindow.Show();

                return newWindow;
            }

            throw new InvalidOperationException("Unknown type: " + navigationHost.GetType());
        }
    }
}