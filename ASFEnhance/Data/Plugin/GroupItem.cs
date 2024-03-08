namespace ASFEnhance.Data.Plugin;

internal sealed record GroupItem
{
    public string Name { get; set; }
    public ulong GroupId { get; set; }

    public GroupItem(string name, ulong groupId)
    {
        Name = name;
        GroupId = groupId;
    }
}
