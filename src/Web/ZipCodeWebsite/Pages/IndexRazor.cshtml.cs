using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZipCodeLibrary;
using ZipCodeWebsite.Controllers;
using ZipCodeWebsite.Models;

namespace ZipCodeWebsite.Pages
{
    public class IndexRazorModel : PageModel
    {
        [BindProperty]
        public string? searchtext { get; set; }
        
        public ZipCodeModel? ZipCodes { get; private set; }
        
        public void OnGet()
        {
            ZipCodes = ZipCodeLogic.GetZipCodes(searchtext);
        }

        public void OnPost()
        {
            ZipCodes = ZipCodeLogic.GetZipCodes(searchtext);
        }
    }
}