using System.IO.Compression;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Zip.Model;

namespace DatenMeister.Zip.Logic;

public class ZipLogicActionHandler : IActionHandler
{
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(_Root.TheOne.__ZipFileExtractAction) == true;
    }

    public async Task<IElement?> Evaluate(ActionLogic actionLogic, IElement action)
    {
        // Caches the stuff
        var sourcePath = action.getOrDefault<string>(_Root._ZipFileExtractAction.sourcePath);
        var targetPath = action.getOrDefault<string>(_Root._ZipFileExtractAction.targetPath);
        var overwriteIfExisting = action.getOrDefault<bool>(_Root._ZipFileExtractAction.overwriteIfExisting);
        var overwriteOnlyIfNewer = action.getOrDefault<bool>(_Root._ZipFileExtractAction.overwriteOnlyIfNewer);
        
        // Creates the result
        var result = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractActionResult);

        // Checks whether the input file is existing at all
        if (!File.Exists(sourcePath))
        {
            throw new InvalidOperationException($"Source Path {sourcePath} does not exist");
        }
        
        // Ok, let's open the Zip file to figure out whether we are having a new date
        using var zipFile = ZipFile.OpenRead(sourcePath);
        
        // Gets the first entry
        var firstFile = zipFile.Entries.FirstOrDefault();
        if (firstFile == null)
        {
            // Nothing to do, we don't have an export
            result.set(_Root._ZipFileExtractActionResult.success, true);
            result.set(_Root._ZipFileExtractActionResult.alreadyExisting, false);
            result.set(_Root._ZipFileExtractActionResult.isAlreadyUpToDate, false);
            return result;
        }
        
        // Get the filename of the entry
        var targetFilePath = Path.Combine(targetPath, firstFile.FullName);

        // Checks, if the zip was already extracted (or it looks like so)
        if (File.Exists(targetFilePath))
        {
            result.set(_Root._ZipFileExtractActionResult.alreadyExisting, true);
            if (!overwriteIfExisting)
            {
                // We may not overwrite
                result.set(_Root._ZipFileExtractActionResult.success, false);
                return result;
            }
        
            if (overwriteOnlyIfNewer && !IsSourceFileNewer(sourcePath, targetFilePath))
            {
                // We are cached
                result.set(_Root._ZipFileExtractActionResult.success, true);
                result.set(_Root._ZipFileExtractActionResult.alreadyExisting, true);
                result.set(_Root._ZipFileExtractActionResult.isAlreadyUpToDate, true);
                return result;
            }
        }
        else
        {
            result.set(_Root._ZipFileExtractActionResult.alreadyExisting, false);
        }

        // Ok, now do the action
        await Task.Run(() =>
            ZipFile.ExtractToDirectory(
                sourcePath, 
                targetPath, 
                true));
        
        result.set(_Root._ZipFileExtractActionResult.success, true);
        result.set(_Root._ZipFileExtractActionResult.isAlreadyUpToDate, false);
        
        File.SetLastWriteTime(targetFilePath, DateTime.Now);
        return result;
    }
    
    private static bool IsSourceFileNewer(string sourcePath, string targetPath)
    {
        return File.GetLastWriteTime(sourcePath) > File.GetLastWriteTime(targetPath);
    }
}