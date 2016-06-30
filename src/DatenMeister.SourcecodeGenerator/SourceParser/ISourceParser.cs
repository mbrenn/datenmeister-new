using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.SourcecodeGenerator.SourceParser
{
    public interface ISourceParser
    {
        /// <summary>
        /// Returns the information whether the given element is a pacage
        ///  </summary>
        /// <param name="element">Element to be queried</param>
        /// <returns>true, if the element is a package</returns>
        bool IsPackage(IObject element);

        /// <summary>
        /// Returns the information whether the given element is a pacage
        ///  </summary>
        /// <param name="element">Element to be queried</param>
        /// <returns>true, if the element is a package</returns>
        bool IsClass(IObject element);
    }
}