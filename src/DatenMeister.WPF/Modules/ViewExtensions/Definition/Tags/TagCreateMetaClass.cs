using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Tags;

public struct TagCreateMetaClass(IElement metaClass)
{
    public IElement MetaClass { get; } = metaClass;

    public override bool Equals(object? obj)
    {
        if (obj is TagCreateMetaClass other)
        {
            return MetaClass.equals(other.MetaClass);
        }

        return false;
    }

    public bool Equals(TagCreateMetaClass other)
    {
        return MetaClass.Equals(other.MetaClass);
    }

    public override int GetHashCode()
    {
        return MetaClass.GetHashCode();
    }
}