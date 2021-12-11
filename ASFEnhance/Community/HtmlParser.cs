#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。

using AngleSharp.Dom;
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Web.Responses;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Chrxw.ASFEnhance.Utils;
using static Chrxw.ASFEnhance.Store.Response;
using Chrxw.ASFEnhance.Localization;
using System.IO;
using System.Text;
using AngleSharp;
using static Chrxw.ASFEnhance.Community.Response;

namespace Chrxw.ASFEnhance.Community
{
    internal static class HtmlParser
    {
        /// <summary>
        /// 判断是否加入群组
        /// </summary>
        /// <param name="response"></param>
        /// <returns>
        /// true:  已加入群组
        /// false: 未加入群组
        /// null:  群组不存在
        /// </returns>
        internal static bool? CheckJoinGroup(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            IElement? anyError = response.Content.SelectSingleNode("//div[@class='error_ctn']");

            if (anyError == null)
            {
                return null;
            }



            return anyError == null;




        }

        /// <summary>
        /// 解析群组列表
        /// </summary>
        /// <returns></returns>
        internal static List<GroupData> ParseGropuList(HtmlDocumentResponse response)
        {
            IEnumerable<IElement> groupNodes = response.Content.SelectNodes("//div[@id='search_results']/div[@id and @class]");

            List<GroupData> gruopInfos = new();

            foreach (IElement groupNode in groupNodes)
            {
                IElement? eleName = groupNode.SelectSingleElementNode(".//a[@class='linkTitle']");
                IElement? eleMembers = groupNode.SelectSingleElementNode(".//a[@class='groupMemberStat linkStandard']");
                IElement? elePublic = groupNode.SelectSingleElementNode(".//span[@class='pubGroup']");
                IElement? eleAction = groupNode.SelectSingleElementNode(".//div[@class='actions']/a[@class='linkStandard']");

                string groupName = eleName?.Text();

                if (string.IsNullOrEmpty(groupName))
                {
                    ASF.ArchiLogger.LogGenericDebug(string.Format("{0} == NULL", nameof(groupName)));
                    continue;
                }

                bool isPublic = elePublic != null;

                string strOnlick = eleAction?.GetAttribute("onclick") ?? "( '0',";

                Match match = Regex.Match(strOnlick, @"\( '(\d+)',");

                if (!match.Success)
                {
                    ASF.ArchiLogger.LogGenericWarning(string.Format("{0} == NULL", nameof(eleName)));
                    continue;
                }
                else
                {
                    string strGroupID = match.Groups[1].ToString();

                    if (!uint.TryParse(strGroupID, out uint groupID))
                    {
                        ASF.ArchiLogger.LogGenericWarning(string.Format("{0} {1} cant parse to uint", nameof(strGroupID), strGroupID));
                        continue;
                    }
                    gruopInfos.Add(new(groupID, groupName, isPublic));
                }
            }
            return gruopInfos;
        }
    }
}
