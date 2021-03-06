﻿using System;
using System.Collections.Generic;

namespace DatenMeister.SourcecodeGenerator
{
    public class SourceGeneratorOptions
    {
        public string ExtentUrl { get; set; } = "dm:///";
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Namespace { get; set; }
        public IEnumerable<Type> Types { get; set; } = new List<Type>();
    }
}