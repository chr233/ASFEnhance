using ASFEnhance.Data.Plugin;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Common;
/// <summary>
/// 
/// </summary>
public sealed record IdData
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("appid")]
    public uint? AppId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("packageid")]
    public uint? PackageId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("bundleid")]
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