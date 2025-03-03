using AngleSharp.Dom;

namespace ASFEnhance.Profile;
internal static class ElementExtensions
{
    /// <summary>
    /// 读取TextContent为int
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defValue"></param>
    /// <returns></returns>
    public static int ReadTextContentAsInt(this IElement? element, int defValue)
    {
        var text = element?.TextContent.Replace(",", "").Replace(".", "");
        if (string.IsNullOrEmpty(text))
        {
            return defValue;
        }
        else
        {
            return int.TryParse(text, out var value) ? value : defValue;
        }
    }

    /// <summary>
    /// 读取TextContent为long
    /// </summary>
    /// <param name="element"></param>
    /// <param name="defValue"></param>
    /// <returns></returns>
    public static long ReadTextContentAsLong(this IElement? element, long defValue)
    {
        var text = element?.TextContent.Replace(",", "").Replace(".", "");
        if (string.IsNullOrEmpty(text))
        {
            return defValue;
        }
        else
        {
            return long.TryParse(text, out var value) ? value : defValue;
        }
    }
}
