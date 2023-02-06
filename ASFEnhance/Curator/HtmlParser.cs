using ASFEnhance.Data;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ASFEnhance.Curator
{
    internal static partial class HtmlParser
    {

        /// <summary>
        /// 解析关注的鉴赏家页
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static HashSet<CuratorItem>? ParseCuratorListPage(AjaxGetCuratorsResponse? response)
        {
            if (response == null)
            {
                return null;
            }

            Match match = MatchCuratorPayload().Match(response.Html);

            if (match.Success)
            {
                try
                {
                    string jsonStr = match.Groups[1].Value;
                    var data = JsonConvert.DeserializeObject<HashSet<CuratorItem>>(jsonStr);
                    return data;
                }
                catch (Exception ex)
                {
                    ASFLogger.LogGenericError(ex.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [GeneratedRegex("g_rgTopCurators = ([^;]+);")]
        private static partial Regex MatchCuratorPayload();
    }
}
