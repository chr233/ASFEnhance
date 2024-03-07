using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASFEnhance.Data;
public sealed record MyGameId
{
    public EGameIdType Type { get; set; }
    public uint Id { get; set; }
}

public enum EGameIdType : byte
{
    AppId,
    PackageId,
    BundleId,
}
