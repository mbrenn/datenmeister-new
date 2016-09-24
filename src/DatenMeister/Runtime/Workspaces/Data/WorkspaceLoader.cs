using System;
using System.Diagnostics;

namespace DatenMeister.Runtime.Workspaces.Data
{
    public class WorkspaceLoader : ObjectFileStorage<WorkspaceFileData>
    {
        public string Filepath { get; set; }

        public IWorkspaceLogic WorkspaceCollection { get; set; }

        public WorkspaceLoader(IWorkspaceLogic workspaceCollection, string filepath)
        {
            Debug.Assert(workspaceCollection != null, "workspaceCollection != null");
            Debug.Assert(filepath != null, "filepath != null");

            WorkspaceCollection = workspaceCollection;
            Filepath = filepath;
        }

        public WorkspaceFileData Load()
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

                    var workspace = new Workspace(workspaceInfo.Id, workspaceInfo.Annotation);
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
            var workSpaceData = new WorkspaceFileData();
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