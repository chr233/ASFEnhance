using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFGifting.Data;

[ProtoContract]
public class AddItemToCartRequest
{
    [ProtoMember(1)]
    public uint SubId { get; set; }

    [ProtoMember(2)]
    public uint BundleId { get; set; }

    [ProtoMember(10)]
    public string Country { get; set; } = "CN";

    [ProtoMember(12)]
    public string Unknown { get; set; } = "";
}
