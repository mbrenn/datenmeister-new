namespace DatenMeister.Core.EMOF.Implementation.Hooks
{
    public interface IResolveHook
    {
        public object? Resolve(ResolveHookParameters hookParameters);
    }
}