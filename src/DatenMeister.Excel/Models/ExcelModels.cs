﻿using System;
using System.Collections.Generic;

namespace DatenMeister.Excel.Models
{
    public static class ExcelModels
    {
        public static IEnumerable<Type> AllTypes => new[]
        {
            typeof(Workbook),
            typeof(Table)
        };
    }
}
