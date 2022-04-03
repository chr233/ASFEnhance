namespace Chrxw.ASFEnhance.Community
{
    internal class Response
    {
        /// <summary>
        /// 单个群组信息
        /// </summary>
        internal sealed class GroupData
        {
            public string Name { get; private set; }
            public ulong GroupID { get; private set; }

            public GroupData(ulong groupID, string name)
            {
                GroupID = groupID;
                Name = name;
            }
        }
    }
}
