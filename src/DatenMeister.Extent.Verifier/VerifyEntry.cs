namespace DatenMeister.Extent.Verifier;

/// <summary>
/// Defines one verify entry
/// </summary>
public record VerifyEntry
{
    /// <summary>
    /// Gets or sets the workspace id to which the entry was created
    /// </summary>
    public string WorkspaceId { get; set; } = string.Empty;
    
    /// <summary>
    /// Get or sets the item uri to which the entry was created
    /// </summary>
    public string ItemUri { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the category
    /// </summary>
    public string Category { get; set; } = string.Empty;
}