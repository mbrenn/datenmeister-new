using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using BurnSystems;

namespace DatenMeister.HtmlEngine
{
    /// <summary>
    /// Defines the class that allows the creation of an html report
    /// </summary>
    public class HtmlReport : IDisposable, IHtmlReport
    {
        /// <summary>
        /// Stores the stream writer instance being used to write the content of the
        /// html report into a file, network or other stream
        /// </summary>
        private TextWriter? _streamWriter;

        /// <summary>
        /// Flag whether the stream writer is disposed
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Gets a list of possible CSS Files
        /// </summary>
        public List<string> CssFiles { get; }= new List<string>();

        /// <summary>
        /// Initializes a new instance of the HtmlReport class
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public HtmlReport(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            _streamWriter = new StreamWriter(stream, Encoding.UTF8);
        }

        /// <summary>
        /// Initializes a new instance of the HtmlReport class
        /// </summary>
        /// <param name="stream">Stream to be used</param>
        public HtmlReport(StreamWriter stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            _streamWriter = stream;
        }

        /// <summary>
        /// Gets a list of CSS Stylesheets to be used
        /// </summary>
        public IList<string> CssStyleSheets { get; set; } = new List<string>();

        /// <summary>
        /// Adds the reference to a css 
        /// </summary>
        /// <param name="cssFile"></param>
        public void AddCssFile(string cssFile)
        {
            CssFiles.Add(cssFile);
        }

        /// <summary>
        /// Adds the reference to a css 
        /// </summary>
        /// <param name="cssStyleSheet">Stylesheet to be added</param>
        public void AddCssStyleSheet(string cssStyleSheet)
        {
            CssStyleSheets.Add(cssStyleSheet);
        }

        /// <summary>
        /// Creates a new HtmlReport and stores the content of the
        /// report within the given file
        /// </summary>
        /// <param name="filePath">Filepath in which the file shall be stored</param>
        public HtmlReport(string filePath)
        {
            _streamWriter = new StreamWriter(filePath, false, Encoding.UTF8);
        }

        /// <summary>
        /// Initializes a new instance of the html writer
        /// </summary>
        /// <param name="writer">Streamwriter to be used</param>
        public HtmlReport(TextWriter writer)
        {
            _streamWriter = writer;
        }

        /// <summary>
        /// Loads and sets the default style
        /// </summary>
        public void SetDefaultCssStyle()
        {
            CssStyleSheets.Add(ResourceHelper.LoadStringFromAssembly(
                typeof(HtmlReport),
                "DatenMeister.HtmlEngine.Css.default_report.css"));
        }

        /// <summary>
        /// Starts the report
        /// </summary>
        public void StartReport(string pageTitle)
        {
            if (_streamWriter == null)
                throw new InvalidOperationException("StreamWriter is not set");
            
            _streamWriter.WriteLine("<!DOCTYPE html>");
            _streamWriter.WriteLine("<html>");
            _streamWriter.WriteLine("  <head>");
            _streamWriter.WriteLine("    <title>" + HttpUtility.HtmlEncode(pageTitle) + "</title>");
            _streamWriter.WriteLine("  </head>");
            foreach (var styleSheet in CssStyleSheets)
            {
                _streamWriter.WriteLine("  <style>");
                _streamWriter.WriteLine(styleSheet);
                _streamWriter.WriteLine("  </style>");
            }

            // Include CSS files
            foreach (var cssFile in CssFiles)
            {
                _streamWriter.WriteLine($"    <link rel=\"stylesheet\" href=\"{cssFile}\" />");
            }

            _streamWriter.WriteLine("  <body>");
        }

        /// <summary>
        /// Adds the element that should be added to the report
        /// </summary>
        /// <param name="elementToBeAdded">Element that should be added
        /// should be an element within this namespace. </param>
        public void Add(HtmlElement elementToBeAdded)
        {
            if (_streamWriter == null)
                throw new InvalidOperationException("StreamWriter is not set");
            
            _streamWriter.WriteLine(elementToBeAdded.ToString());
        }

        /// <summary>
        /// Ends the report
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void EndReport()
        {
            if (_isDisposed || _streamWriter == null)
            {
                throw new InvalidOperationException("The Report is already disposed");
            }

            _streamWriter.WriteLine("  </body>");
            _streamWriter.WriteLine("</html>");

            Dispose();
        }

        /// <summary>
        /// Disposes the report
        /// </summary>
        public void Dispose()
        {
            _streamWriter?.Dispose();
            _isDisposed = true;
            _streamWriter = null;
        }
    }
}