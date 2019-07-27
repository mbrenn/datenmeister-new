using System.Net;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlElement
    {
        protected HtmlElement()
        {
                
        }
        
        public static implicit operator HtmlElement(string value)
        {
            return new HtmlRawString(WebUtility.HtmlEncode(value));
        }
    }
}