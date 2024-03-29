﻿using System;
using BurnSystems.Logging;

namespace DatenMeister.Core.Runtime.Workspaces.Data
{
    public class WorkspaceLoader : ObjectFileStorage<WorkspaceFileData>
    {
        private static readonly ClassLogger Logger = new(typeof(WorkspaceLoader));

        /// <summary>
        /// Stores the configuration of the workspace loader
        /// </summary>
        public WorkspaceLoaderConfig Config { get; set; }

        /// <summary>
        /// Stores the workspace logic storing the workspaces in memory
        /// </summary>
        public IWorkspaceLogic WorkspaceLogic { get; set; }

        public WorkspaceLoader(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            WorkspaceLogic = workspaceLogic ?? throw new ArgumentNullException(nameof(workspaceLogic));
            Config = scopeStorage.Get<WorkspaceLoaderConfig>();
        }

        /// <summary>
        /// Loads the workspaces from the given file.
        /// If the workspace is already existing, the annotation will be overridden.
        /// If not, it will be created.
        /// </summary>
        /// <returns>The loaded workspace</returns>
        public WorkspaceFileData? Load()
        {
            try
            {
                var workspaceData = Load(Config.filepath);

                if (workspaceData == null)
                {
                    // Not existing
                    return null;
                }

                foreach (var workspaceInfo in workspaceData.workspaces)
                {
                    var foundWorkspace = WorkspaceLogic.GetWorkspace(workspaceInfo.id);
                    if (foundWorkspace != null)
                    {
                        // Already exists, update annotation
                        foundWorkspace.annotation = workspaceInfo.annotation;
                        continue;
                    }

                    var workspace = new Workspace(workspaceInfo.id, workspaceInfo.annotation);
                    WorkspaceLogic.AddWorkspace(workspace);
                }

                return workspaceData;
            }
            catch (Exception e)
            {
                Logger.Error($"Loading of workspaces failed: {e}");
                return null;
            }
        }

        /// <summary>
        /// Stores the workspaces in the file by converting them into the the workspace filedata and then moving it into
        /// the file by using the ObjectFileStorage
        /// </summary>
        public void Store()
        {
            var workSpaceData = new WorkspaceFileData();
            foreach (var workSpace in WorkspaceLogic.Workspaces)
            {
                workSpaceData.workspaces.Add(
                    new WorkspaceInfo(workSpace.id, workSpace.annotation));
            }

            Save(Config.filepath, workSpaceData);
        }
    }
}