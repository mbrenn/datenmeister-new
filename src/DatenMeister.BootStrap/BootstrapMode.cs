namespace DatenMeister.BootStrap;

/// <summary>
/// Defines the bootstrap mode being used to import the first classes.
/// </summary>
public enum BootstrapMode
{
    /// <summary>
    /// Primitive Types, Uml and Mof will be imported and will be considered as completely self-hostimng
    /// </summary>
    Mof,

    /// <summary>
    /// Only primitive types and uml will be imported. These will be linked to the MOF classes
    /// </summary>
    Uml,

    /// <summary>
    /// A slim integration but still mof
    /// </summary>
    SlimMof,

    /// <summary>
    /// A slim integration but uml
    /// </summary>
    SlimUml
}