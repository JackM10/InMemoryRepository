using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview
{
    public class InMemoryRepository<T> : IRepository<T> where T : IStoreable
    {
        private ConcurrentDictionary<IComparable, T> ConcurrentInternalStorage = new ConcurrentDictionary<IComparable, T>();

        public IEnumerable<T> All()
        {
            return ConcurrentInternalStorage.Values;
        }

        public void Delete(IComparable id)
        {
            ConcurrentInternalStorage.TryRemove(id, out _);
        }

        public T FindById(IComparable id)
        {
            T value;
            ConcurrentInternalStorage.TryGetValue(id, out value);

            return value;
        }

        public void Save(T item)
        {
            if (ConcurrentInternalStorage.ContainsKey(item.Id)) // for highConcurrency replace with AddOrUpdate()
            {
                T value;
                ConcurrentInternalStorage.TryGetValue(item.Id, out value);
                ConcurrentInternalStorage.TryUpdate(item.Id, item, value);
            }
            else
                ConcurrentInternalStorage.TryAdd(item.Id, item);
        }
    }
}
