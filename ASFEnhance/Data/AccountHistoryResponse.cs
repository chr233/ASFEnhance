using Newtonsoft.Json;

namespace ASFEnhance.Data
{
    internal sealed class AccountHistoryResponse
    {
        [JsonProperty(PropertyName = "html", Required = Required.Always)]
        public string HtmlContent { get; set; }

        [JsonProperty(PropertyName = "cursor", Required = Required.DisallowNull)]
#pragma warning disable CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
        public CursorData? Cursor { get; set; }
#pragma warning restore CS8632 // 只能在 "#nullable" 注释上下文内的代码中使用可为 null 的引用类型的注释。
    }

}
