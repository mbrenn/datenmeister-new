﻿using System;

namespace DatenMeisterWPF.Forms.Base.ViewExtensions
{
    public class GenericButtonDefinition : ViewExtension
    {
        public GenericButtonDefinition(string name, Action onPressed)
        {
            Name = name;
            OnPressed = onPressed;
        }

        public string Name { get; }

        public Action OnPressed { get; }

        public override string ToString()
        {
            return $"GenericButtonDefinition: {Name}";
        }
    }
}