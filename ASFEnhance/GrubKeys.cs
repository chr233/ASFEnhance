using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Chrxw.ASFEnhance
{
    internal static class GrubKeys
    {
        public static ArrayList String2keysArray(string payload)
        {
            ArrayList result = new ArrayList();
            string pattern = @"[0-9A-Z]{5}-[0-9A-Z]{5}-[0-9A-Z]{5}";
            MatchCollection list = Regex.Matches(payload, pattern, RegexOptions.IgnoreCase);
            for (int i = 0; i < list.Count; i++)
            {
                Match match = list[i];
                result.Add(match.Value);
            }
            return result;
        }
        public static string? String2keysString(string payload)
        {
            ArrayList result = String2keysArray(payload);

            StringBuilder rs = new StringBuilder();
            if (result != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    rs.Append(result[i] + Environment.NewLine);
                }
                return rs.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}
