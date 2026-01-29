using ArchiSteamFarm.Web.Responses;
using ASFEnhance.Data;

namespace ASFEnhance.Cart;

internal static class HtmlParser
{
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
