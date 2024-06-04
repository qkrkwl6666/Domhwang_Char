public abstract class DataTable
{
    public readonly string FormatPath = "DataTable/{0}";
    public readonly string FormatPath2 = "Assets/Resources/DataTable/{0}";
    public abstract void Load(string Path);
}
