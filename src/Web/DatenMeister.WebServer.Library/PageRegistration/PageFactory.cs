using System;
using System.IO;

namespace DatenMeister.WebServer.Library.PageRegistration
{
    public record PageFactory(string Url, string ContentType, Func<Stream> PageStreamFunction);
}