namespace DatenMeister.WPF.Forms.Base;

public class PropertyValueChangedEventArgs(DetailFormControl formControl, string propertyName) : EventArgs
{
    public DetailFormControl FormControl
    {
        get;
        set;
    } = formControl;

    /// <summary>
    /// Gets or sets the name of the property that has changed
    /// </summary>
    public string PropertyName = propertyName;

    /// <summary>
    /// Gets or sets the new value
    /// </summary>
    public object? NewValue { get; set; }
}