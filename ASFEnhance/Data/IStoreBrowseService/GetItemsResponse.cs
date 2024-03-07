using Newtonsoft.Json;

namespace ASFEnhance.Data.IStoreBrowseService;
/// <summary>
/// 
/// </summary>
public sealed record GetItemsResponse
{
    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("store_items")]
    public List<StoreItemData>? StoreItems { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record StoreItemData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("item_type")]
        public int ItemType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id")]
        public uint Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("success")]
        public byte Success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("visible")]
        public bool Visiable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("store_url_path")]
        public string? StoreUrlPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("appid")]
        public uint AppId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public byte Type { get; set; }
    }
}
