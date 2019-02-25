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
            ConcurrentInternalStorage.TryGetValue(id, out var value);

            return value;
        }

        public void Save(T item)
        {
            if(item != null)
                ConcurrentInternalStorage.AddOrUpdate(item.Id, item, (key, value) => item);
        }
    }
}
