using DatenMeister.Core;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Web.Json;

public class MofJsonDeconverter(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : DirectJsonDeconverter(workspaceLogic, scopeStorage);