#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using ASFEnhance.Localization;
using System.Text;
using System.Text.RegularExpressions;
using static ASFEnhance.Utils;

namespace ASFEnhance.Community
{
    internal sealed class HtmlParser
    {
        /// <summary>
        /// 获取群组名
        /// </summary>
        /// <param name="response"></param>
        /// <returns>
        /// true,string:  群组名
        /// false,string: 群组未找到/网络错误
        /// </returns>
        internal static (bool, string) GetGroupName(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return (false, Langs.CartNetworkError);
            }

            IElement? groupNameNode = response.Content.SelectSingleNode("//div[@class='grouppage_resp_title ellipsis']");

            if (groupNameNode != null)
            {
                string groupName = groupNameNode.TextContent.Trim().Replace("\t\t\t\t", " ");

                return (true, groupName);
            }
            else
            {
                IElement? errorMessage = response.Content.SelectSingleNode("//div[@class='error_ctn']//h3");

                return (false, errorMessage.TextContent.Trim());
            }
        }

        /// <summary>
        /// 判断是否已加入群组
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static JoinGroupStatus CheckJoinGroup(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            IElement? joinAction = response.Content.SelectSingleNode("//div[@class='grouppage_join_area']/a");

            if (joinAction != null)
            {
                string link = joinAction.GetAttribute("href");

                return link.StartsWith("javascript") ? JoinGroupStatus.Unjoined : JoinGroupStatus.Joined;
            }
            else
            {
                return JoinGroupStatus.Applied;
            }
        }


        /// <summary>
        /// 解析群组列表
        /// </summary>
        /// <returns></returns>
        internal static string? ParseGropuList(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IEnumerable<IElement> groupNodes = response.Content.SelectNodes("//div[@id='search_results']/div[@id and @class]");

            if (!groupNodes.Any())
            {
                return Langs.GroupListEmpty;
            }

            StringBuilder result = new();

            result.AppendLine(Langs.MultipleLineResult);
            result.AppendLine(Langs.GroupListTitle);

            int i = 1;

            foreach (IElement groupNode in groupNodes)
            {
                IElement? eleName = groupNode.SelectSingleElementNode(".//a[@class='linkTitle']");
                IElement? eleAction = groupNode.SelectSingleElementNode(".//div[@class='actions']/a");

                string groupName = eleName?.Text();

                if (string.IsNullOrEmpty(groupName))
                {
                    ASFLogger.LogGenericDebug(string.Format("{0} == NULL", nameof(groupName)));
                    continue;
                }

                string strOnlick = eleAction?.GetAttribute("onclick") ?? "( '0',";

                Match match = Regex.Match(strOnlick, @"\( '(\d+)',");

                if (!match.Success)
                {
                    ASFLogger.LogGenericWarning(string.Format(Langs.SomethingIsNull, nameof(eleName)));
                    continue;
                }
                else
                {
                    string strGroupID = match.Groups[1].ToString();

                    if (!ulong.TryParse(strGroupID, out ulong groupID))
                    {
                        ASFLogger.LogGenericWarning(string.Format("{0} {1} cant parse to uint", nameof(strGroupID), strGroupID));
                        continue;
                    }
                    result.AppendLine(string.Format(Langs.GroupListItem, i++, groupName, groupID));
                }
            }
            return result.ToString();
        }
    }
}
