namespace GaldevWeb;

public class TabConvertingFileDataProvider : JsonPath.IDataProvider
{
    public string GetData(string key)
    {
        var data = System.IO.File.ReadAllText(key);
        return data.Replace("\t", "  ");
    }

    public bool HasData(string key)
    {
        return System.IO.File.Exists(key);
    }

    public void SetData(string key, string value)
    {
        throw new NotImplementedException();
    }
}
