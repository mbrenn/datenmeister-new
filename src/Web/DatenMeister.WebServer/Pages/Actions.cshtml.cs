using Autofac;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Types;
using DatenMeister.WebServer.Shared;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages;

public class ActionsModel(IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
    public void OnGet()
    {
    }

    public string GetZipCodeMetaClassUri()
    {
        var dm = GiveMe.Scope;
        var localTypeSupport = dm.Resolve<LocalTypeSupport>();
        return localTypeSupport.GetMetaClassFor(typeof(ZipCode))?.GetUri()
               ?? throw new InvalidOperationException("Zipcode extension was not found");
    }
}