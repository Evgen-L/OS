using System.Collections.Generic;
using System.Linq;

namespace Determination
{
    internal static class DictionaryExtension
    {
        public static Dictionary<T1, T2> AddAsFirst<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value) => 
            (new Dictionary<T1, T2> {{key,value}}).Concat(dict).ToDictionary(k => k.Key, v => v.Value);
    }
}