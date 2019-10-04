using System;

namespace DatenMeister.Modules.HtmlReporter.HtmlEngine
{
    public class HtmlHeadline : HtmlElement
    {
        public HtmlHeadline(object headline, int level)
        {
            if (level < 1 || level > 6)
            {
                throw new InvalidOperationException("Level of headline is invalid");
            }
            
            Level = level;
            Headline = headline;
        }

        /// <summary>
        /// Stores the text of the headline
        /// </summary>
        public object Headline { get; }

        /// <summary>
        /// Stores the level of the headline
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// Converts the element to a string containing the html for the headline
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"<h{Level}>{Headline}</h{Level}>";
        }
    }
}