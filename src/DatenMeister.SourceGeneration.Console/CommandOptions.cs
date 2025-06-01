using BurnSystems.CommandLine.ByAttributes;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace DatenMeister.SourceGeneration.Console;

public class CommandOptions
{
    [UnnamedArgument(
        Index = 0,
        HelpText = "Path to Xmi File",
        IsRequired = true)]
    public string PathXml { get; set; } = string.Empty;

    [UnnamedArgument(
        Index = 1,
        HelpText = "Path to which the value shall be targeted",
        IsRequired = true)]
    public string PathTarget { get; set; } = string.Empty;

    [UnnamedArgument(
        Index = 2,
        HelpText = "Namespace of the target class",
        IsRequired = true)]
    public string CodeNamespace { get; set; } = string.Empty;

    [UnnamedArgument(
        Index = 3,
        HelpText = "Overriding Namespace of the xmi")]
    public string XmiNamespace { get; set; } = string.Empty;
}