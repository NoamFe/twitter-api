using System.Collections.Concurrent;

namespace Twitter.Data;

public static class ConcurrentDictionaryExtensions
{
    public static bool TryUpdate<TKey, TValue>(
             this ConcurrentDictionary<TKey, TValue> dictionary,
             TKey key,
             TValue newValue,
             Func<TValue, TValue, TValue> updateFactory)
    {
        TValue curValue;
       
        while (dictionary.TryGetValue(key, out curValue))
        {
            if (dictionary.TryUpdate(key, updateFactory(curValue, newValue), curValue))
                return true;
        }
        return false;
    }

    public static bool TryAdd<TKey, TValue>(
             this ConcurrentDictionary<TKey, TValue> dictionary,
             TKey key,
             TValue newValue,
             Func<TValue, TValue, TValue> updateFactory)
    {
        TValue curValue;
       
        while (!dictionary.TryGetValue(key, out curValue))
        {
            if (dictionary.TryAdd(key, newValue))
                return true;
            else
            {
                return dictionary.TryUpdate(key, newValue, updateFactory);
            }
        }
        return false;
    }
}
