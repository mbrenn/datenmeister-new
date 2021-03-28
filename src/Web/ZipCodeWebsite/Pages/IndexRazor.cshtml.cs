using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            ZipCodes = ZipCodeHandler.GetZipCodes(searchtext);
        }

        public void OnPost()
        {
            ZipCodes = ZipCodeHandler.GetZipCodes(searchtext);
        }
    }
}