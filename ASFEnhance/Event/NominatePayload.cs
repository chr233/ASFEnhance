using ProtoBuf;

namespace ASFEnhance.Event;

/// <summary>
/// 提名载荷
/// </summary>
[ProtoContract]
internal sealed record NominatePayload
{
    [ProtoMember(1)]
    public int CategoryId { get; set; }
    [ProtoMember(2)]
    public int NominatedId { get; set; }
    [ProtoMember(3)]
    public int Source { get; set; }
}