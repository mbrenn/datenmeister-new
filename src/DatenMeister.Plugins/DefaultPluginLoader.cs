using System.Reflection;
using BurnSystems.Logging;

namespace DatenMeister.Plugins;

/// <summary>
///     Loads the plugins
/// </summary>
public class DefaultPluginLoader : IPluginLoader
{
    /// <summary>
    ///     Allows logging of events within the plugin loader
    /// </summary>
    private static readonly ClassLogger Logger = new(typeof(DefaultPluginLoader));

    public DefaultPluginSettings Settings { get; set; } = new();

    /// <summary>
    ///     Gets the list of loaded
    /// </summary>
    /// <returns>List of types which belong to a plugin</returns>
    public List<Type> GetPluginTypes()
    {
        var pluginList = new List<Type>();

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            try
            {
                // Go through all types and check, if the type has implemented the interface for the plugging
                pluginList.AddRange(
                    assembly.GetTypes()
                        .Where(type =>
                            type.GetInterfaces().Any(x => x == typeof(IDatenMeisterPlugin))
                            && !pluginList.Any(x => x.FullName != null && x.FullName.Equals(type.FullName))));
            }
            catch (ReflectionTypeLoadException e)
            {
                Logger.Error(
                    $"PluginLoader: Exception during assembly loading of {assembly.FullName} [{assembly.Location}]: {e.Message}");
            }

        return pluginList.OrderBy(x => x.FullName).ToList();
    }

    /// <summary>
    ///     Loads all assemblies from the specific folder into the current context
    /// </summary>
    /// <param name="path">Path to directory whose assemblies are loaded</param>
    public void LoadAssembliesFromFolder(string path)
    {
        if (!Path.IsPathRooted(path)) path = Path.Combine(Environment.CurrentDirectory, path);

        if (Directory.Exists(path))
        {
            var files = Directory.GetFiles(path)
                .Where(x => Path.GetExtension(x).ToLower() == ".dll");
            foreach (var file in files)
            {
                var filenameWithoutExtension = Path.GetFileNameWithoutExtension(file).ToLower();

                if (!IsManagedAssembly(file))
                {
                    Logger.Info($"Skipped since it is not a managed .dll: {filenameWithoutExtension}");
                    continue;
                }

                if (Settings.AssemblyFilesToBeSkipped.Contains(Path.GetFileName(file)))
                {
                    Logger.Info($"Skipped by Settings: {filenameWithoutExtension}");
                    continue;
                }

                if (IsDotNetLibrary(filenameWithoutExtension))
                    // Skip .Net Library
                    continue;

                if (AppDomain.CurrentDomain.GetAssemblies().All(
                        x => x.GetName().Name?.ToLower() != filenameWithoutExtension))
                {
                    var assemblyName = AssemblyName.GetAssemblyName(Path.Combine(path, file));

                    try
                    {
                        var assembly = Assembly.Load(assemblyName);
                        Logger.Info($"Loaded (1): {assembly.GetName().Name}, {assembly.GetName().Version}");
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"Loading of assembly {file} ({assemblyName}) failed: {e}");
                    }
                }
            }
        }
        else
        {
            Logger.Warn($"Directory does not exist: {path}");
        }

#if DEBUG
        var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(x => x.FullName);
        var duplicates = loadedAssemblies.GroupBy(x => x.FullName).Where(x => x.Count() > 1).Select(x => x.Key);
        foreach (var duplicate in duplicates.Where(x => x != null))
            Logger.Error($"Duplicate Assembly detected: {duplicate}");
#endif
    }

    /// <summary>
    ///     Checks whether the given assembly is a managed assembly
    ///     Copied from
    ///     https://stackoverflow.com/questions/367761/how-to-determine-whether-a-dll-is-a-managed-assembly-or-native-prevent-loading
    /// </summary>
    /// <param name="fileName">Name of the file to be checked</param>
    /// <returns>true, if the assembly a managed assembly</returns>
    private static bool IsManagedAssembly(string fileName)
    {
        using Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        using var binaryReader = new BinaryReader(fileStream);

        if (fileStream.Length < 64) return false;

        //PE Header starts @ 0x3C (60). Its a 4 byte header.
        fileStream.Position = 0x3C;
        var peHeaderPointer = binaryReader.ReadUInt32();
        if (peHeaderPointer == 0) peHeaderPointer = 0x80;

        // Ensure there is at least enough room for the following structures:
        //     24 byte PE Signature & Header
        //     28 byte Standard Fields         (24 bytes for PE32+)
        //     68 byte NT Fields               (88 bytes for PE32+)
        // >= 128 byte Data Dictionary Table
        if (peHeaderPointer > fileStream.Length - 256) return false;

        // Check the PE signature.  Should equal 'PE\0\0'.
        fileStream.Position = peHeaderPointer;
        var peHeaderSignature = binaryReader.ReadUInt32();
        if (peHeaderSignature != 0x00004550) return false;

        // skip over the PEHeader fields
        fileStream.Position += 20;

        const ushort pe32 = 0x10b;
        const ushort pe32Plus = 0x20b;

        // Read PE magic number from Standard Fields to determine format.
        var peFormat = binaryReader.ReadUInt16();
        if (peFormat != pe32 && peFormat != pe32Plus) return false;

        // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
        // When this is non-zero then the file contains CLI data otherwise not.
        var dataDictionaryStart = (ushort) (peHeaderPointer + (peFormat == pe32 ? 232 : 248));
        fileStream.Position = dataDictionaryStart;

        var cliHeaderRva = binaryReader.ReadUInt32();
        return cliHeaderRva != 0;
    }


    /// <summary>
    ///     Loads all assemblies from the current directories
    /// </summary>
    public void LoadAllAssembliesFromCurrentDirectory()
    {
        var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                            ?? throw new InvalidOperationException("directoryName is not set");
        LoadAssembliesFromFolder(directoryName);
    }

    /// <summary>
    ///     Loads all referenced assemblies from the assemblies being currently loaded.
    /// </summary>
    public void LoadAllReferencedAssemblies()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                     .Where(x => !IsDotNetLibrary(x.GetName())))
            LoadReferencedAssembly(assembly);
    }

    /// <summary>
    ///     Loads all referenced assembly of the given assembly and all subassemblies
    /// </summary>
    /// <param name="assembly">Assembly whose references shall be loaded</param>
    private void LoadReferencedAssembly(Assembly assembly)
    {
        // All assemblies, which do not start with Microsoft or System.
        // We will not find any extent or something like that within these assemblies.
        foreach (var name in assembly.GetReferencedAssemblies()
                     .Where(x => !IsDotNetLibrary(x))
                     .Where(x => AppDomain.CurrentDomain.GetAssemblies().All(a =>
                         !string.Equals(a.GetName().Name, x.Name, StringComparison.CurrentCultureIgnoreCase))))
        {
            var innerAssembly = Assembly.Load(name);
            Logger.Info($"Loaded (2): {innerAssembly}");
            LoadReferencedAssembly(innerAssembly);
        }
    }


    /// <summary>
    ///     Gets true, if the given library is a dotnet library which
    ///     starts with System, Microsoft or mscorlib.
    /// </summary>
    /// <param name="assemblyName">Name of the assembly</param>
    /// <returns></returns>
    public static bool IsDotNetLibrary(AssemblyName assemblyName)
    {
        return assemblyName.FullName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
               assemblyName.FullName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
               assemblyName.FullName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    ///     Gets true, if the given library is a dotnet library which
    ///     starts with System, Microsoft or mscorlib.
    /// </summary>
    /// <param name="assemblyName">Name of the assembly</param>
    /// <returns>true, if the given library is a .Net Library</returns>
    public static bool IsDotNetLibrary(string assemblyName)
    {
        return assemblyName.StartsWith("microsoft", StringComparison.InvariantCultureIgnoreCase) ||
               assemblyName.StartsWith("mscorlib", StringComparison.InvariantCultureIgnoreCase) ||
               assemblyName.StartsWith("system", StringComparison.InvariantCultureIgnoreCase);
    }
}