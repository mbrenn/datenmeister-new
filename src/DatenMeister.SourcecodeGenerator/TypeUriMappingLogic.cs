using System.Reflection;
using BurnSystems.Logging;
using DatenMeister.Core.Interfaces;

namespace DatenMeister.SourcecodeGenerator;

/// <summary>
/// Performs the mapping between the Uri of the type and the actual full name of the class
/// This is used for the wrapper to directly provide the right class for the wrapper for
/// referenced or contained items
/// </summary>
public class TypeUriMappingLogic
{
    /// <summary>
    /// Defines the classlogger 
    /// </summary>
    public static ILogger Logger { get; } = new ClassLogger(typeof(TypeUriMappingLogic));
    
    public class Entry
    {
        public string TypeUri { get; set; } = string.Empty;

        public string ClassFullName { get; set; } = string.Empty;

        public TypeKind TypeKind { get; set; }
    }

    public List<Entry> Entries { get; set; } = [];

    public void LoadAssemblies(string directory = "")
    {
        if (string.IsNullOrEmpty(directory))
        {
            // Gets the directory of the assembly itself
            directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) 
                                    ?? throw new InvalidOperationException("Could not determine assembly directory");
        }
        
        foreach (var file in Directory.GetFiles(directory, "*.dll"))
        {
            // Loads the attributes TypeUriAttribute from all classes in file and adds them to the entry
            // Load the assembly
            try
            {
                var assembly = Assembly.LoadFrom(file);
                var current = Entries.Count;

                // Get all types in the assembly
                foreach (var type in assembly.GetTypes())
                {
                    // Look for TypeUriAttribute on the type
                    var attribute = type.GetCustomAttribute<TypeUriAttribute>();
                    if (attribute != null)
                    {
                        // Add to entries
                        Entries.Add(new Entry
                        {
                            TypeUri = attribute.Uri,
                            ClassFullName = (type.FullName ?? type.Name).Replace('+', '.'),
                            TypeKind = attribute.TypeKind
                        });
                    }
                }

                /*Console.WriteLine(
                    $"Loaded {Entries.Count - current} types from {Path.GetFileName(file)} with {Entries.Count} entries in total.");*/
            }
            catch (Exception ex)
            {
                // Log or handle exceptions when loading assemblies
                Console.WriteLine($"Error loading assembly {file}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Updates the entry, if existing. If the element is not existing, a new entry will be added
    /// </summary>
    /// <param name="typeUri">Type of the Uri</param>
    /// <param name="fullName">Fullname</param>
    public void UpdateEntry(string? typeUri, string fullName, TypeKind typeKind)
    {
        if (string.IsNullOrEmpty(typeUri))
        {
            return;
        }
        
        var found =  Entries.FirstOrDefault(x => x.TypeUri == typeUri && x.TypeKind == typeKind);
        if (found != null)
        {
            var before = found.ClassFullName;
            found.ClassFullName = fullName;
            if (before != found.ClassFullName)
            {
                Logger.Trace($"{typeKind} conversion: {before} -->  {found.ClassFullName}");
            }
        }
        else
        {
            var newEntry = new Entry
            {
                TypeUri = typeUri,
                ClassFullName = fullName,
                TypeKind = TypeKind.ClassTree
            };
            
            Entries.Add(newEntry);
            Logger.Trace($"{typeKind} addition  : {newEntry.ClassFullName}");
        }
    }
}