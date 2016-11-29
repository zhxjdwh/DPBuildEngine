using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTest.Common
{
    /// <summary>
    /// 只读ConcurrentDictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ReadOnlyConcurrentDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private ReadOnlyConcurrentDictionary()
        {
            
        }
        public ReadOnlyConcurrentDictionary(IEnumerable<KeyValuePair<TKey,TValue>> collections):base(collections)
        {
        }

        private new TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory,
            Func<TKey, TValue, TValue> updateValueFactory)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new void Clear()
        {
            throw new InvalidOperationException("can not clear a readonly collection");
        }
        private new TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new TValue GetOrAdd(TKey key, TValue value)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new bool TryAdd(TKey key, TValue value)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new bool TryRemove(TKey key, out TValue value)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
        private new bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            throw new InvalidOperationException("can not mofidy a readonly collection");
        }
    }
}
