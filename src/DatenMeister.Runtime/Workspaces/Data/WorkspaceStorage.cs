using System;
using System.Diagnostics;

namespace DatenMeister.Runtime.Workspaces.Data
{
    public class WorkspaceStorage : ObjectFileStorage<WorkspaceData>, IDisposable
    {
        public string Filepath { get; set; }

        public IWorkspaceCollection WorkspaceCollection { get; set; }

        public WorkspaceStorage(IWorkspaceCollection workspaceCollection, string filepath)
        {
            Debug.Assert(workspaceCollection != null, "workspaceCollection != null");
            Debug.Assert(filepath != null, "filepath != null");

            WorkspaceCollection = workspaceCollection;
            Filepath = filepath;
        }

        public void Dispose()
        {
            var workSpaceData = new WorkspaceData();
            foreach (var workSpace in  this.WorkspaceCollection.Workspaces)
            {
                workSpaceData.Workspaces.Add(new WorkspaceInfo()
                {
                    Title = workSpace.id
                });

            }

            Save(Filepath, workSpaceData);
        }
    }
}