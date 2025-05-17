namespace DatenMeister.SourcecodeGenerator
{
    public class SourceGeneratorOptions
    {
        public string ExtentUrl { get; set; } = "dm:///";
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Namespace { get; set; } = string.Empty;

        public IEnumerable<Type> Types { get; set; } = new List<Type>();
    }
}