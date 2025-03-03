using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;
using System.Text;

namespace ASFEnhance.Cart;

internal static class HtmlParser
{
    /// <summary>
    /// 解析购物车可用区域
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    internal static string? ParseCartCountries(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var currentCountry = response.Content.QuerySelector("#usercountrycurrency");

        var availableCountries = response.Content.QuerySelectorAll("#usercountrycurrency_droplist>li>a");

        StringBuilder message = new();
        message.AppendLine(Langs.MultipleLineResult);

        if (currentCountry != null)
        {
            message.AppendLine(Langs.MultipleLineResult);
            message.AppendLine(Langs.AvailableAreaHeader);

            string? currentCode = currentCountry.GetAttribute("value");

            foreach (var availableCountrie in availableCountries)
            {
                string? countryCode = availableCountrie.GetAttribute("id");
                string countryName = availableCountrie.TextContent ?? "";

                if (!string.IsNullOrEmpty(countryCode) && countryCode != "help")
                {
                    message.AppendLineFormat(currentCode == countryCode ? Langs.AreaItemCurrent : Langs.AreaItem, countryCode, countryName);
                }
            }
        }
        else
        {
            message.AppendLine(Langs.NoAvailableArea);
        }

        return message.ToString();
    }

    internal static List<DigitalGiftCardOption>? ParseDigitalGiftCardOptions(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var result = new List<DigitalGiftCardOption>();

        var cardsEle = response.Content.QuerySelectorAll("div.giftcard_amounts>.giftcard_selection");

        foreach (var cardEle in cardsEle)
        {
            var nameEle = cardEle.QuerySelector(".giftcard_text");
            var name = nameEle?.TextContent;

            var linkEle = cardEle.QuerySelector("a");
            var link = linkEle?.GetAttribute("href");

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(link))
            {
                continue;
            }

            var match = RegexUtils.MatchCardBalance().Match(link);

            if (match.Success)
            {
                var value = match.Groups[1].Value;
                if (uint.TryParse(value, out uint balance))
                {
                    result.Add(new DigitalGiftCardOption
                    {
                        Name = name,
                        Balance = balance,
                    });
                }
            }
        }

        return result;
    }

    internal static Dictionary<string, string>? FetchPayload(HtmlDocumentResponse? response)
    {
        if (response?.Content == null)
        {
            return null;
        }

        var result = new Dictionary<string, string>();

        var inputs = response.Content.QuerySelectorAll("#externalForm>input");

        foreach (var input in inputs)
        {
            var key = input.GetAttribute("name");
            var value = input.GetAttribute("value");
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                result.Add(key, value);
            }
        }

        return result;
    }
}
