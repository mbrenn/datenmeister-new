using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Shared;

namespace DatenMeister.WebServer.Pages;

public class TestCases(IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    public void OnGet()
    {
            
    }
}