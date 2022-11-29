using ProtoBuf;

namespace ASFEnhance.Event
{
    [ProtoContract]
    internal sealed class GetDiscoveryQueueResponse
    {
        [ProtoMember(1)]
        public List<uint> Appids { get; set; }
        [ProtoMember(2)]
        public string CountryCode { get; set; }
    }
}
