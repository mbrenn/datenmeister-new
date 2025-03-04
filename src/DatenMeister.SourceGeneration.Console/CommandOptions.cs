﻿using CommandLine;

namespace DatenMeister.SourceGeneration.Console;

public class CommandOptions
{
    [Value(0, MetaName = "Path to Xmi")]
    public string PathXml { get; set; }


    [Value(1, MetaName = "Path to which the value shall be targeted")]
    public string PathTarget { get; set; }

    [Value(2, MetaName = "Namespace of the target class")]
    public string Namespace { get; set; }
}