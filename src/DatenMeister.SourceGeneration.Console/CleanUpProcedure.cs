using System;
using System.Data.Common;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Extensions;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;

namespace DatenMeister.SourceGeneration.Console;

public static class CleanUpProcedure
{
    private const string ExtentUri = "dm:///_internal/types/internal";
    public static async Task CleanUpExtent(string fileName, bool dryRun)
    {
        await using var dm = await GiveMe.DatenMeister();

        var typeExtent = new MofUriExtent(
            XmiProvider.CreateByFile(fileName),
            "dm:///_internal/types/internal", null);

        dm.WorkspaceLogic.AddExtent(dm.WorkspaceLogic.GetDataWorkspace(), typeExtent);
        
        // Ok, now get the extent
        var extent = dm.WorkspaceLogic.GetDataWorkspace().FindExtent(ExtentUri)
                     ?? throw new InvalidOperationException("The extent could not be found.");
        
        // Now perform the cleaning
        var defaultValueStripper = new DefaultValueStripper();
        defaultValueStripper.StripDefaultValues(extent, dryRun);
        
        var extentManager = dm.Resolve<ExtentManager>();
        await extentManager.StoreExtent(typeExtent);
    }
}