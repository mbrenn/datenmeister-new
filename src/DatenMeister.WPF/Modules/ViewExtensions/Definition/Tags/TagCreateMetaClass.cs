using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Modules.ViewExtensions.Definition.Tags
{
    public struct TagCreateMetaClass
    {
        public TagCreateMetaClass(IElement metaClass)
        {
            MetaClass = metaClass;
        }

        public IElement MetaClass { get; }

        public override bool Equals(object? obj)
        {
            if (obj is TagCreateMetaClass other)
            {
                return MetaClass?.@equals(other.MetaClass) == true;
            }

            return false;
        }

        public bool Equals(TagCreateMetaClass other)
        {
            return MetaClass.Equals(other.MetaClass);
        }

        public override int GetHashCode()
        {
            return MetaClass?.GetHashCode() ?? base.GetHashCode();
        }
    }
}