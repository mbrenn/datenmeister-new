namespace DatenMeister.WebServer.Library.PageRegistration;

/// <summary>
/// Defines the page factory to be used 
/// </summary>
/// <param name="Url">Url under which the page shall be accessible</param>
/// <param name="ContentType">The mime content type to be used</param>
/// <param name="PageStreamFunction">A factory function to create the page</param>
public record PageFactory(string Url, string ContentType, Func<Stream> PageStreamFunction);