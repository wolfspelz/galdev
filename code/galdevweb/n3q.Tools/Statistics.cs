using System.Collections.Generic;

namespace n3q.Tools
{
    public class Statistics
    {
        readonly object _mutex = new object();
        readonly Dictionary<string, long> _long = new Dictionary<string, long>();
        readonly Dictionary<string, string> _string = new Dictionary<string, string>();

        public void Set(string name, long value)
        {
            lock (_mutex) {
                if (_long.ContainsKey(name)) {
                    _long.Remove(name);
                }
                _long.Add(name, value);
            }
        }

        public void Set(string name, string value)
        {
            lock (_mutex) {
                if (_string.ContainsKey(name)) {
                    _string.Remove(name);
                }
                _string.Add(name, value);
            }
        }

        public void Increment(string name)
        {
            lock (_mutex) {
                if (!_long.ContainsKey(name)) {
                    _long.Add(name, 0);
                }
                _long[name] += 1;
            }
        }

        public void Decrement(string name)
        {
            lock (_mutex) {
                if (!_long.ContainsKey(name)) {
                    _long.Add(name, 0);
                }
                _long[name] -= 1;
            }
        }

        public void Delete(string name)
        {
            lock (_mutex) {
                if (_long.ContainsKey(name)) {
                    _long.Remove(name);
                }
                if (_string.ContainsKey(name)) {
                    _string.Remove(name);
                }
            }
        }

        public Dictionary<string, string> Data
        {
            get {
                var l = new Dictionary<string, string>();

                foreach (var pair in _long) {
                    l.Add(pair.Key, pair.Value.ToString());
                }
                foreach (var pair in _string) {
                    l.Add(pair.Key, pair.Value);
                }

                return l;
            }
        }

    }
}
