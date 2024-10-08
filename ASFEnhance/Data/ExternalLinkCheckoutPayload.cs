namespace ASFEnhance.Data;

/// <summary>
///     外部支付请求体
/// </summary>
sealed record ExternalLinkCheckoutPayload
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="formUrl"></param>
    /// <param name="payload"></param>
    public ExternalLinkCheckoutPayload(Uri formUrl, Dictionary<string, string> payload)
    {
        FormUrl = formUrl;
        Payload = payload;
    }

    public Uri FormUrl { get; init; }
    public Dictionary<string, string> Payload { get; init; }
}