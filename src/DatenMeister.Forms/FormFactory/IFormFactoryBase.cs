namespace DatenMeister.Forms.FormFactory;


/// <summary>
/// Gives guidance to potential form factory orders in which the
/// form factories are called
/// </summary>
public static class FormFactoryPriorities
{
    public const int Default = 0;

    /// <summary>
    /// This should be the first activity in which the form is being prepared
    /// </summary>
    public const int Preparation = 500;

    /// <summary>
    /// Creates the build up of the form which provides the base structure
    /// </summary>
    public const int PrimaryBuildUp = 200;

    /// <summary>
    /// Allows to add the buttons which contain the primary function for the user
    /// </summary>
    public const int AdditionalFunctionsPrimary = 100;
    
    /// <summary>
    /// Allows to add the buttons which contain the secondary function for the user
    /// </summary>
    public const int AdditionalFunctionsSecondary = 50;
    
    /// <summary>
    /// Allows to add the buttons which contain the secondary function for the user
    /// </summary>
    public const int CleanUp = 0;
    
    /// <summary>
    /// Contains all the rest which will be called at last
    /// </summary>
    public const int Miscellaneous = -100;
}

/// <summary>
/// Defines the order of the form factory
/// </summary>
public interface IFormFactoryBase
{
    /// <summary>
    /// ´Gets the order of the FormFactory. May be 0, if there is none
    /// </summary>
    public int Priority { get; }
}

/// <summary>
/// Gives the default implementation for the order. 
/// </summary>
public class FormFactoryBase : IFormFactoryBase
{
    public int Priority { get; set; }
}