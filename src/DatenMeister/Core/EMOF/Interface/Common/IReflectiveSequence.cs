namespace DatenMeister.Core.EMOF.Interface.Common
{
    public interface IReflectiveSequence : IReflectiveCollection
    {
        void add(int index, object value);

        object get(int index);

        void remove(int index);

        object? set(int index, object value);
    }
}