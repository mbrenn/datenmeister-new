using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Json
{
    public class MofJsonDeconverter : DirectJsonDeconverter
    {
        public MofJsonDeconverter(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage) 
            : base (workspaceLogic, scopeStorage)
        {
            
        }
    }
}