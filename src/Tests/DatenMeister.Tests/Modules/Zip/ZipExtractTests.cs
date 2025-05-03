using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DatenMeister.Actions;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Zip.Model;
using NUnit.Framework;

namespace DatenMeister.Tests.Modules.Zip;

[TestFixture]
public class ZipExtractTests
{
    [Test]
    public async Task TestZipExtract()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        if (File.Exists(TargetPathFile))
        {
            File.Delete(TargetPathFile);
        }

        var action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, false);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, false);
        var result = await actionLogic.ExecuteAction(action);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.success), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.alreadyExisting), Is.False);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.isAlreadyUpToDate), Is.False);
        
        File.Delete(TargetPathFile);
    }

    [Test]
    public async Task TestZipExtractCached()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        if (File.Exists(TargetPathFile))
        {
            File.Delete(TargetPathFile);
        }
        
        // Creates the file
        var action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, false);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, false);
        await actionLogic.ExecuteAction(action);
        
        Thread.Sleep(1000);
        
        // Now do the same one second later and check that file was cached.
        action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, true);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, true);
        var result = await actionLogic.ExecuteAction(action);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.success), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.alreadyExisting), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.isAlreadyUpToDate), Is.True);
        
        Thread.Sleep(100);
        
        // Checks, updates Last Writing time in past and checks that the wroting is correctly being doen
        File.SetLastWriteTime(TargetPathFile, DateTime.Now.Subtract(TimeSpan.FromMinutes(30)));
        
        action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, true);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, true);
        result = await actionLogic.ExecuteAction(action);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.success), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.alreadyExisting), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.isAlreadyUpToDate), Is.False);
        
        File.Delete(TargetPathFile);
    }

    [Test]
    public async Task TestZipExtractNoOverwrite()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        if (File.Exists(TargetPathFile))
        {
            File.Delete(TargetPathFile);
        }
        
        // Creates the file
        var action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, false);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, false);
        await actionLogic.ExecuteAction(action);
        
        // Now do the same one second later and check that file was cached.
        action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, true);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, false);
        var result = await actionLogic.ExecuteAction(action);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.success), Is.False);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.alreadyExisting), Is.True);
        
        File.Delete(TargetPathFile);
    }

    [Test]
    public async Task TestZipExtractForceOverwrite()
    {
        // Check that the ZipLoading can be found in the local types
        await using var dm = await DatenMeisterTests.GetDatenMeisterScope();
        var actionLogic = new ActionLogic(dm.WorkspaceLogic, dm.ScopeStorage);

        if (File.Exists(TargetPathFile))
        {
            File.Delete(TargetPathFile);
        }
        
        // Creates the file
        var action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, false);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, false);
        await actionLogic.ExecuteAction(action);
        
        // Now do the same one second later and check that file was cached.
        action = InMemoryObject.CreateEmpty(_Root.TheOne.__ZipFileExtractAction);
        action.set(_Root._ZipFileExtractAction.sourcePath, ZipFilePath);
        action.set(_Root._ZipFileExtractAction.targetPath, TargetPathDirectory);
        action.set(_Root._ZipFileExtractAction.overwriteOnlyIfNewer, false);
        action.set(_Root._ZipFileExtractAction.overwriteIfExisting, true);
        var result = await actionLogic.ExecuteAction(action);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.success), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.alreadyExisting), Is.True);
        Assert.That(result.getOrDefault<bool>(_Root._ZipFileExtractActionResult.isAlreadyUpToDate), Is.False);
        
        File.Delete(TargetPathFile);
    }

    private static string ZipFilePath
    {
        get
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(currentDirectory!, "Examples/plz.zip");
        }
    }
    
    private static string TargetPathDirectory
    {
        get
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var result = Path.Combine(currentDirectory!, "Output/extract/");
            Directory.CreateDirectory(result);
            return result;
        }
    }

    private static string TargetPathFile => Path.Combine(TargetPathDirectory, "plz.csv");
}