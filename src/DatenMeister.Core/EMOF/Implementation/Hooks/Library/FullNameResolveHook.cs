using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.EMOF.Implementation.Hooks.Library
{
    /// <summary>
    /// Performs are solution according the fullname by using the fn parameter
    /// </summary>
    public class FullNameResolveHook : IResolveHook
    {
        /// <inheritdoc></inheritdoc>
        public object? Resolve(ResolveHookParameters hookParameters)
        {
            var fullName =
                hookParameters.QueryString["fn"];

            if (fullName == null)
            {
                return hookParameters.CurrentItem;
            }

            return hookParameters.CurrentItem switch {
                
                IExtent extent =>
                    NamedElementMethods.GetByFullName(extent, fullName),
                IReflectiveCollection reflectiveCollection =>
                    NamedElementMethods.GetByFullName(reflectiveCollection, fullName),
                _ => hookParameters.CurrentItem
            };
        }
    }
}