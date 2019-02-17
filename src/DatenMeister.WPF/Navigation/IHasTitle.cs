namespace DatenMeister.WPF.Navigation
{
    /// <summary>
    /// Defines the interface to retrieve the title of a component.
    /// This class is used by windows to get information about the element
    /// to be showed and to put it into the window title. 
    /// </summary>
    public interface IHasTitle
    {
        string Title { get; }
    }
}