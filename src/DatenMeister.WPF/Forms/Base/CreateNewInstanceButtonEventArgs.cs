using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base;

public class CreateNewInstanceButtonEventArgs(IElement? selectedType) : EventArgs
{
    public IElement? SelectedType { get; } = selectedType;
}