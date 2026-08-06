using System.Text.Json;
using System.Text.Json.Serialization;

namespace ASFEnhance.Data.Convertor;

/// <summary>
/// 支持从 number 或 string 反序列化为 string 的自定义转换器
/// </summary>
internal sealed class NumberOrStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            // 尝试读取为整数或浮点数，然后转换为字符串（保持原始数值表示）
            if (reader.TryGetInt64(out var l))
            {
                return l.ToString();
            }

            if (reader.TryGetDouble(out var d))
            {
                return d.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            if (reader.TryGetDecimal(out var dm))
            {
                return dm.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }

            // 最后作为原始文本返回
            return reader.GetString();
        }

        // 其它类型（如 true/false/object/array）返回原始文本
        return reader.GetString();
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        // 始终以字符串写出（与期望的宽容读取保持一致）
        writer.WriteStringValue(value);
    }
}