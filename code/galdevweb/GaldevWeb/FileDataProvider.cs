namespace GaldevWeb
{
    public class FileDataProvider : JsonPath.IDataProvider
    {
        public string GetData(string key)
        {
            return System.IO.File.ReadAllText(key);
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
}
