using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Core.EMOF.Implementation.DotNet;

public class WorkspaceDotNetHelper
{
    /// <summary>
    /// Gets the metaclass uri of a certain dot net type
    /// </summary>
    /// <param name="type">Type to be handled</param>
    /// <param name="metaWorkspace">The metaworkspace for the dotnet type</param>
    /// <returns></returns>
    public static string? GetMetaClassUriOfDotNetType(IWorkspace metaWorkspace, Type type)
    {
        foreach (var metaExtent in metaWorkspace.extent.OfType<MofExtent>())
        {
            var element = metaExtent.TypeLookup.ToElement(type);
            if (!string.IsNullOrEmpty(element))
            {
                return element;
            }
        }

        return null;
    }
        
    /// <summary>
    /// Gets the metaclass uri of a certain dot net type
    /// </summary>
    /// <param name="metaClassUri">Metaclass to be queried</param>
    /// <param name="metaWorkspace">The metaworkspace for the dotnet type</param>
    /// <returns></returns>
    public static Type? GetDotNetTypeOfMetaClassUri(IWorkspace metaWorkspace, string metaClassUri)
    {
        return metaWorkspace.extent.OfType<MofExtent>()
            .Select(metaExtent => metaExtent.TypeLookup.ToType(metaClassUri))
            .FirstOrDefault(element => element != null);
    }
}