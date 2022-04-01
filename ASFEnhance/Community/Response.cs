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
                GroupID = groupID;
                Name = name;
                IsPublic = isPublic;
            }
        }
    }
}
