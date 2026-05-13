using BurnSystems.CommandLine.ByAttributes;

namespace DatenMeister.Action.Executor;

public class Arguments
{
    [UnnamedArgument(HelpText = "Xmi File containing the action", IsRequired = true)]
    public string XmiFileName { get; set;} = string.Empty;
    
    [UnnamedArgument(HelpText = "Path of action to be executed (e.g. #transform). This can also be an absolute Url", IsRequired = true)]
    public string ActionPath { get; set; } = string.Empty;
}