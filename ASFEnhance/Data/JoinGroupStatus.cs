namespace ASFEnhance.Data;

internal enum JoinGroupStatus
{
    /// <summary>出错</summary>
    Failed = -1,
    /// <summary>未加入</summary>
    NotJoined = 0,
    /// <summary>已加入</summary>
    Joined = 1,
    /// <summary>已申请加入</summary>
    Applied = 2,
}
