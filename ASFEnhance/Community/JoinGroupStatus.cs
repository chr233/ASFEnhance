using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrxw.ASFEnhance.Community
{
    /// <summary>
    /// 加入群组状态
    /// </summary>
    internal enum JoinGroupStatus
    {
        /// <summary>出错</summary>
        Failed = -1,
        /// <summary>未加入</summary>
        Unjoined = 0,
        /// <summary>已加入</summary>
        Joined =1,
        /// <summary>已申请加入</summary>
        Applied = 2,
    }
}
