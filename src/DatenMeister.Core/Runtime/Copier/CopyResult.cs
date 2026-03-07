namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Defines the structure that is used to return information of the copying
/// </summary>
public struct CopyResult
{
    /// <summary>
    /// Defines the type of the copy that had been executed
    /// </summary>
    public CopyType CopyType;

    /// <summary>
    /// Returns the direct result in case there is no CopyType.FindClonedReference returned
    /// </summary>
    public object? DirectResult;

    /// <summary>
    /// Returns the indirect result in case there is a CopyType.FindClonedReference returned
    /// </summary>
    public Func<object?>? IndirectResult;

    public static CopyResult CreateResultForInstance(object? result)
    {
        return new CopyResult
        {
            CopyType = CopyType.Clone,
            DirectResult = result
        };
    }
        
    public static CopyResult CreateResultForReference(object? reference)
    {
        return new CopyResult
        {
            CopyType = CopyType.FindClonedReference,
            DirectResult = reference
        };
    }

    public static CopyResult CreateResultForToFindClonedReference(Func<object?> reference)
    {
        return new CopyResult
        {
            CopyType = CopyType.KeepReference,
            IndirectResult = reference
        };
    }
}