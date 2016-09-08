using System;
using System.Diagnostics;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Runtime.Workspaces.Data
{
    public class WorkspaceLoader : ObjectFileStorage<WorkspaceData>
    {
        public string Filepath { get; set; }

        public IWorkspaceCollection WorkspaceCollection { get; set; }

        public WorkspaceLoader(IWorkspaceCollection workspaceCollection, string filepath)
        {
            Debug.Assert(workspaceCollection != null, "workspaceCollection != null");
            Debug.Assert(filepath != null, "filepath != null");

            WorkspaceCollection = workspaceCollection;
            Filepath = filepath;
        }

        public WorkspaceData Load()
        {
            try
            {
                var loaded = Load(Filepath);

                if (loaded == null)
                {
                    // Not existing
                    return null;
                }

            
                foreach (var workspaceInfo in loaded.Workspaces)
                {
                    if (WorkspaceCollection.GetWorkspace(workspaceInfo.Id) != null)
                    {
                        // Already exists
                        continue;
                    }

                    var workspace = new Workspace<IExtent>(workspaceInfo.Id, workspaceInfo.Annotation);
                    WorkspaceCollection.AddWorkspace(workspace);
                }

                return loaded;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Loading of workspaces failed: {e}");
                return null;
            }
        }

        public void Store()
        {
            var workSpaceData = new WorkspaceData();
            foreach (var workSpace in  WorkspaceCollection.Workspaces)
            {
                workSpaceData.Workspaces.Add(new WorkspaceInfo
                {
                    Id = workSpace.id,
                    Annotation = workSpace.annotation
                });
            }

            Save(Filepath, workSpaceData);
        }
    }
}