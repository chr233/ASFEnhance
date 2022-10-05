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

    [ProtoContract]
    internal sealed class SkipDiscoveryQueueItemRequest
    {
        [ProtoMember(1)]
        public int QueueType { get; set; } = 1;
        [ProtoMember(2)]
        public uint Appid { get; set; }
        [ProtoMember(3)]
        public StorePageFilter Filter { get; set; } = new();

    }

    [ProtoContract]
    internal sealed class StorePageFilter
    {
        [ProtoMember(1)]
        public UnknownMsg Msg { get; set; } = new();
    }

    [ProtoContract]
    internal sealed class UnknownMsg
    {
        [ProtoMember(1)]
        public int Value { get; set; } = 1235711;
    }

}
