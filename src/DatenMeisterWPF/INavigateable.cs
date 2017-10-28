using System;
using System.Windows.Controls;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF
{
    public interface INavigateable
    {
        IControlNavigation NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode);
    }
}