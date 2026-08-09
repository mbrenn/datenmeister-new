using DatenMeister.Core.Interfaces;
using DatenMeister.WebServer.Shared;

namespace DatenMeister.WebServer.Pages;

public class LoggingModel(IScopeStorage scopeStorage) : _Layout(scopeStorage)
{
}