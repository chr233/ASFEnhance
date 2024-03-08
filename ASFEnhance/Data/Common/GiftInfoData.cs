using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed partial record GiftInfoData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("accountid_giftee")]
    public ulong AccountIdGiftee { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("gift_message")]
    public GiftMessageData? GiftMessage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("time_scheduled_send")]
    public ulong TimeScheduledSend { get; set; }
}