﻿using System;

namespace DatenMeister.ManualMapping
{
    public class PropertyMapping
    {
        public Action<object, object> SetValueFunc { get; set; }
        public Func<object, object> GetValueFunc { get; set; }
        public object DefaultValue { get; set; }
    } 
}