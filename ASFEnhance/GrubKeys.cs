using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chrxw.ASFEnhance
{
    internal static class GrubKeys
    {
        // Ã·»°KEY
        public static string? GrubKeysFromString(string payload)
        {
            List<string> keys = new();

            MatchCollection matches;
            matches = Regex.Matches(payload, @"[\S\d]{5}-[\S\d]{5}-[\S\d]{5}", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                keys.Add(match.Value.ToUpperInvariant());
            }

            matches = Regex.Matches(payload, @"\s([\S\d]{15})\s", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                keys.Add(groups[1].Value.ToUpperInvariant().Insert(10, "-").Insert(5, "-"));
            }

            return keys.Count > 0 ? string.Join('\n', keys) : null;
        }
    }
}
