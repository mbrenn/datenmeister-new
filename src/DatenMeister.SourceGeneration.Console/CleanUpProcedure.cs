using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using DatenMeister.Core.Extensions;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;

namespace DatenMeister.SourceGeneration.Console;

public static class CleanUpProcedure
{
    public static async Task CleanUpExtent(string fileName, string extentUri, bool dryRun)
    {
        await using var dm = await Program.GiveMeDatenMeister();
        var extentManager = dm.Resolve<ExtentManager>();
        
        var absolutePath = Path.Combine(Environment.CurrentDirectory, fileName);

        var result = await extentManager.CreateAndAddXmiExtent(
            extentUri, 
            absolutePath);

        var typeExtent = result.Extent
                         ?? throw new InvalidOperationException("Loading was not successful.");
        
        // Now perform the cleaning
        var defaultValueStripper = new DefaultValueStripper();
        defaultValueStripper.StripDefaultValues(typeExtent, dryRun);
        
        await extentManager.StoreExtent(typeExtent);
        
        await extentManager.RemoveExtent(typeExtent);
    }
}