using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base;

public class CreateNewInstanceButtonEventArgs : EventArgs
{
    public CreateNewInstanceButtonEventArgs(IElement? selectedType)
    {
        SelectedType = selectedType;
    }

    public IElement? SelectedType { get; }
}