namespace DatenMeister.Forms;

/// <summary>
/// Defines the result of the form creation.
/// It contains the created form, but also additional information whether
/// the factory has managed the request or finalized the request
/// </summary>
public class FormCreationResult
{
    /// <summary>
    /// Gets or sets a value if the request has been managed.
    /// If the request is not managed until the end of the chain, an error will be sent out
    /// </summary>
    public bool IsManaged { get; set; }
    
    /// <summary>
    /// Gets or sets the information whether the request is finalized
    /// </summary>
    public bool IsFinalized { get; set; }
}