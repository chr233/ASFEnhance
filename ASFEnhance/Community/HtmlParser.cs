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

namespace Chrxw.ASFEnhance.Community
{
    internal static class HtmlParser
    {
        //解析商店页
        internal static bool? CheckJoinGroup(HtmlDocumentResponse response)
        {
            if (response == null)
            {
                return null;
            }

            using (FileStream fs = new("1.html", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter sw = new(fs, Encoding.UTF8))
                {
                    response.Content.ToHtml(sw);
                }
            }

            IElement? anyError = response.Content.SelectSingleNode("//div[@class='error_ctn']");

            return anyError == null;
        }
    }
}
