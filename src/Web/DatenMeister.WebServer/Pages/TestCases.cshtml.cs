using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class TestCases(IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    public void OnGet()
    {
            
    }
}