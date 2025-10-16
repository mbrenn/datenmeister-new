using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Models;

namespace DatenMeister.Excel.Helper;

public class ExcelColumnTranslator
{
    /// <summary>
    /// Translates the column names
    /// </summary>
    private readonly List<ExcelImporter.Column> _translateColumnNames = new();

    public void LoadTranslation(IElement configurationSetting)
    {
        var columns =
            configurationSetting.getOrDefault<IReflectiveCollection>(_ExtentLoaderConfigs._ExcelLoaderConfig.columns);
        if (columns != null)
        {
            foreach (var column in columns.OfType<IElement>())
            {
                _translateColumnNames.Add(
                    new ExcelImporter.Column(
                        column.getOrDefault<string>(_ExtentLoaderConfigs._ExcelColumn.name),
                        column.getOrDefault<string>(_ExtentLoaderConfigs._ExcelColumn.header)
                    ));
            }
        }
    }

    /// <summary>
    /// Translates the header into a name
    /// </summary>
    /// <param name="headerName">Name of the header</param>
    /// <returns>Translated Header NAme</returns>
    public string TranslateHeader(string headerName)
    {
        var found = _translateColumnNames.FirstOrDefault(x => x.Header == headerName);
        if (found != null)
        {
            // Gets the translated header name
            return found.Name;
        }

        // Gets the original name
        return headerName;
    }

    /// <summary>
    /// Translates the header into a name. If the given translation entry is not found, 
    /// then a null value is returned.
    /// </summary>
    /// <param name="headerName">Name of the header</param>
    /// <returns>Translated Header NAme</returns>
    public string? TranslateHeaderOrNull(string headerName)
    {
        var found = _translateColumnNames.FirstOrDefault(x => x.Header == headerName);
        if (found != null)
        {
            // Gets the translated header name
            return found.Name;
        }

        return null;
    }
}