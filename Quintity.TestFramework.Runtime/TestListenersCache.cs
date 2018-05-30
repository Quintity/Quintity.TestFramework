using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quintity.TestFramework.Runtime
{
    public static class TestListenersCache
    {
        private static Dictionary<string, object> _cache;
        public static Dictionary<string, object> Cache => _cache;

        private static object lock1 = new object();
        public static void Add(KeyValuePair<string, object> keyValuePair)
        {
            Add(keyValuePair.Key, keyValuePair.Value);
        }

        public static void Add(string key, object value)
        {
            lock (lock1)
            {
                _cache = _cache ?? new Dictionary<string, object>();
                _cache.Add(key, value);
            }
        }

        public static bool TryGetValueAsString(string key, out string value)
        {
            var found = TryGetValue(key, out object @object);
            value = found ? @object as string : null;
            
            return found;
        }
        private static object lock2 = new object();
        public static bool TryGetValue(string key, out object value)
        {
            lock (lock2)
            {
                _cache = _cache ?? new Dictionary<string, object>();
                return _cache.TryGetValue(key, out value);
            }
        }
    }
}
