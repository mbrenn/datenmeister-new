using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.SourcecodeGenerator.SourceParser;

public interface ISourceParser
{
    /// <summary>
    /// Returns the information whether the given element is a package
    ///  </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>true, if the element is a package</returns>
    bool IsPackage(IObject element);

    /// <summary>
    /// Returns the information whether the given element is a class
    ///  </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>true, if the element is a package</returns>
    bool IsClass(IObject element);

    /// <summary>
    /// Returns the information whether the given element is an enumeration
    /// </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>true, if the element is an Enumeration</returns>
    bool IsEnum(IObject element);

    /// <summary>
    /// Returns the information whether the given element is an enumeration litral
    /// </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>true, if the element is an EnumLiteral</returns>
    bool IsEnumLiteral(IObject element);

    /// <summary>
    /// Returns the information whether the given element is a property
    ///  </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>true, if the element is a package</returns>
    bool IsProperty(IObject element);

    /// <summary>
    /// Returns the information whether the given element is a primitive type
    /// </summary>
    /// <param name="element">Element to be queried</param>
    /// <returns>True, if the element is a primitive type</returns>
    bool IsPrimitiveType(IObject element);
}