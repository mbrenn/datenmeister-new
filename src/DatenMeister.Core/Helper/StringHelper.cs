using System.Globalization;

namespace DatenMeister.Core.Helper;

public static class StringHelper
{
    /// <summary>
    /// A helper method to capitalize the first letter of a string
    /// </summary>
    /// <param name="input">Input string</param>
    /// <returns>Output string</returns>
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToUpper(input[0], CultureInfo.InvariantCulture) + input[1..];
    }
}