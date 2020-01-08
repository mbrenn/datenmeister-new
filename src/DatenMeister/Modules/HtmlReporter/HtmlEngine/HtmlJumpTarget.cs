using System.Net;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlJumpTarget : HtmlElement
    {
        private readonly string _id;
        private readonly object _text;

        public HtmlJumpTarget(string id, object text)
        {
            id = id.Replace(" ", string.Empty);
            _id = id;
            this._text = text;
        }

        public override string ToString()
        {
            return $"<a name=\"{WebUtility.HtmlEncode(_id)}\" />{_text}";
        }
    }
}