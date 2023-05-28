using System.Collections.Generic;

namespace n3q.Tools
{
    public interface ICallbackConfig
    {
        object Get(string name, object defaultValue);
        void Set(string name, object value);
    }

    public class MemoryCallbackConfig : ICallbackConfig
    {
        public Dictionary<string, object> Data = new Dictionary<string, object>();

        public object Get(string name, object defaultValue)
        {
            if (Data.ContainsKey(name))
            {
                return Data[name];
            }

            //throw new NotImplementedException();
            return defaultValue;
        }

        public void Set(string name, object value)
        {
            Data[name] = value;
        }
    }
}
