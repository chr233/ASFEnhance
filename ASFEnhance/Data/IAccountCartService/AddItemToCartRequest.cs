using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASFEnhance.Data.IStoreBrowseService.GetItemsResponse;

namespace ASFEnhance.Data.IAccountCartService;
/// <summary>
/// 
/// </summary>
public sealed record AddItemToCartRequest
{

    /// <summary>
    /// 
    /// </summary>
    [JsonProperty("response")]
    public ResponseData? Response { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public sealed record ResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("store_items")]
        public List<StoreItemData>? StoreItems { get; set; }
    }

}
