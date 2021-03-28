﻿using System.Collections.Generic;

namespace ZipCodeWebsite.Models
{
    /*
     interface ZipCodeData {
        items: Array<ZipCode>;
        truncated: boolean;
        noItemFound: boolean;
    }

    interface ZipCode{
        id: number;
        name: string;
        zip: number;
        positionLong: number;
        positionLat: number;
    }*/
    
    public class ZipCodeModel
    {
        public List<ZipCodeData> items { get; set; } = new List<ZipCodeData>();
        public bool truncated { get; set; }
        public bool noItemFound { get; set; }
    }

    public class ZipCodeData
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public string zip { get; set; } = "";
        public double positionLong { get; set; }
        public double positionLat { get; set; }
    }
}