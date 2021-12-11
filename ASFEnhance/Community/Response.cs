using System.Collections.Generic;

namespace Chrxw.ASFEnhance.Community
{
    internal class Response
    {
        //单个群组信息
        internal sealed class GroupData
        {
            public uint GroupID { get; private set; }
            public string Name { get; private set; }
            public bool IsPublic { get; private set; }

            public GroupData(uint groupID, string name,  bool isPublic)
            {
                this.GroupID = groupID;
                this.Name = name;
                this.IsPublic = isPublic;
            }
        }
    }
}
