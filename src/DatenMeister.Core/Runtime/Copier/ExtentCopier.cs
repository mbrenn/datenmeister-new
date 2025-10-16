using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Runtime.Copier;

/// <summary>
/// Supports the copying of an extent
/// </summary>
public class ExtentCopier(IFactory factory)
{
    private readonly IFactory _factory = factory ?? throw new InvalidOperationException(nameof(factory));

    public void Copy(IExtent source, IExtent target, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;
        var sourceSequence = source.elements();
        var targetSequence = target.elements();

        Copy(sourceSequence, targetSequence, copyOptions);
    }

    public void Copy(IReflectiveCollection sourceSequence, IReflectiveCollection targetSequence, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;

        var copier = new ObjectCopier(_factory);
        foreach (var copiedElement in sourceSequence
                     .Select(element => element as IElement)
                     .Select(elementAsElement => copier.Copy(elementAsElement!, copyOptions)))
        {
            targetSequence.add(copiedElement);
        }
    }

    public void Copy(IEnumerable<object> sourceSequence, IReflectiveCollection targetSequence, CopyOption? copyOptions = null)
    {
        copyOptions ??= CopyOptions.None;

        var copier = new ObjectCopier(_factory);
        foreach (var copiedElement in sourceSequence
                     .Select(element => element as IElement)
                     .Where(x => x != null)
                     .Select(elementAsElement => copier.Copy(elementAsElement!, copyOptions)))
        {
            targetSequence.add(copiedElement);
        }
    }
}