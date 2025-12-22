using System.IO.Compression;
using DatenMeister.Actions;
using DatenMeister.Actions.ActionHandler;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Zip.Model;

namespace DatenMeister.Zip.Logic;

/// <summary>
/// This class handles the extraction of zip files within the DatenMeister environment.
/// </summary>
public class ZipLogicActionHandler : IActionHandler
{
    /// <summary>
    /// Determines whether this action handler is responsible for the given action node.
    /// </summary>
    /// <param name="node">The node to check.</param>
    /// <returns>True, if the node represents a ZipFileExtractAction.</returns>
    public bool IsResponsible(IElement node)
    {
        return node.getMetaClass()?.equals(_Root.TheOne.__ZipFileExtractAction) == true;
    }

    /// <summary>
    /// Executes the zip extraction logic for the given action.
    /// </summary>
    /// <param name="actionLogic">The action logic context.</param>
    /// <param name="action">The element containing the action parameters.</param>
    /// <returns>A result element indicating the outcome of the extraction.</returns>
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
    
    /// <summary>
    /// Checks whether the source zip file is newer than the already extracted target file.
    /// </summary>
    /// <param name="sourcePath">The path to the source zip file.</param>
    /// <param name="targetPath">The path to the extracted target file.</param>
    /// <returns>True, if the source file is newer than the target file.</returns>
    private static bool IsSourceFileNewer(string sourcePath, string targetPath)
    {
        // The write time within the ZIP File (the source)
        var writeTimeSourcePath = File.GetLastWriteTime(sourcePath);
        
        // The last write time within the target directory
        var writeTimeTargetPath = File.GetLastWriteTime(targetPath);

        return writeTimeSourcePath > writeTimeTargetPath;
    }
}