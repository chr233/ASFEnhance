using ASFEnhance.Data.Plugin;
using Newtonsoft.Json;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed record IdData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("appid")]
    public uint? AppId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("packageid")]
    public uint? PackageId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("bundleid")]
    public uint? BundleId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IdData() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="KeyNotFoundException"></exception>
    public IdData(SteamGameId id)
    {
        switch (id.Type)
        {
            case ESteamGameIdType.App:
                AppId = id.Id;
                break;
            case ESteamGameIdType.Sub:
                PackageId = id.Id;
                break;
            case ESteamGameIdType.Bundle:
                BundleId = id.Id;
                break;
            default:
                throw new KeyNotFoundException();
        }
    }
}