using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed partial record GiftInfoData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("accountid_giftee")]
    public ulong AccountIdGiftee { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("gift_message")]
    public GiftMessageData? GiftMessage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("time_scheduled_send")]
    public ulong TimeScheduledSend { get; set; }
}