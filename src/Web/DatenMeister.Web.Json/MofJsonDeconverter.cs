using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Web.Json;

public class MofJsonDeconverter(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : DirectJsonDeconverter(workspaceLogic, scopeStorage);