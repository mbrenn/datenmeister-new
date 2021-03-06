﻿namespace DatenMeister.Excel.Helper
{
    public class ExcelImportLoaderConfig : ExcelLoaderConfig
    {
        /// <summary>
        /// Gets or sets the path of the excel import settings in which the create Xmi-Extent shall be stored
        /// </summary>
        public string? extentPath { get; set; }

        public ExcelImportLoaderConfig()
        {
            
        }
        
        public ExcelImportLoaderConfig(string extentUri) : base(extentUri)
        {
        }
    }
}