﻿namespace DatenMeister.Web.Models.PostModels
{
    public class ItemSetPropertyModel : ItemReferenceModel
    {
        public string property { get; set; }

        public string newValue { get; set; }
    }
}