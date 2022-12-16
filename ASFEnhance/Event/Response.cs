using ProtoBuf;

namespace ASFEnhance.Event
{
    [ProtoContract]
    internal sealed class GetDiscoveryQueueResponse
    {
        [ProtoMember(1)]
        public List<uint> Appids { get; set; } = new();

        [ProtoMember(2)]
        public string CountryCode { get; set; } = "";
    }
}
