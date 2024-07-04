using System.Globalization;
using System.Text;

namespace Domain.Extension;

public static class StringExtension
{
    public static bool CompareUpperCase(this string origin, string searchFor)
    {
        var index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(origin, searchFor, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

        return index >= 0;
    }

    public static string RemoveCharacter(this string txt)
    {
        return new string(txt.Normalize(NormalizationForm.FormD).Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark).ToArray());
    }
}