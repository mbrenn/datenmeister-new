using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DatenMeister.WPF.Modules
{
    public class StandardRepository : IIconRepository
    {
        public ImageSource GetIcon(string name)
        {
            try
            {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.UriSource =
                        new Uri("pack://application:,,,/DatenMeisterWPF;component/assets/icons/dialog-question.png");
                    image.EndInit();
                    return image;
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error during loading of " + name + ": " + exc.Message);
                return null;
            }
        }
    }
}