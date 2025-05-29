using BurnSystems;

namespace DatenMeister.Reports;

public class ReportHelper
{
    /// <summary>
    /// Creates a random file and returns the stream writing instance
    /// to the file.
    /// </summary>
    /// <param name="directoryPath">Sets the directory path</param>
    /// <param name="path">The path to the created file</param>
    /// <returns>The stream-writer for the file</returns>
    public static TextWriter CreateRandomFile(out string path, string? directoryPath = null)
    {
        if (directoryPath == null)
        {
            path = StringManipulation.RandomString(10) + ".html";
        }
        else
        {
            path = Path.Combine(directoryPath, StringManipulation.RandomString(10) + ".html");
        }

        return new StreamWriter(path);
    }
}