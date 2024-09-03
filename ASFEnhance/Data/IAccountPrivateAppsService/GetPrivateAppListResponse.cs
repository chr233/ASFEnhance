using System.Text.Json.Serialization;

namespace ASFEnhance.Data.IAccountPrivateAppsService;
internal sealed record GetPrivateAppListResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("private_apps")]
    public AppListData? PrivateApps { get; set; }

    public sealed record AppListData
    {
        [JsonPropertyName("appids")]
        public List<uint>? AppIds { get; set; }
    }
}
