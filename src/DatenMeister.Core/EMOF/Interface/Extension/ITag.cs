using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Interface.Extension
{
    /// <summary>
    ///     Defines the Tag interface as defined in MOF CoreSpecification 2.5
    /// </summary>
    // TODO: Inherit from IElement
    public interface ITag
    {
        string name { get; set; }

        string value { get; set; }

        IReflectiveCollection elements { get; set; }

        IElement owner { get; set; }
    }
}