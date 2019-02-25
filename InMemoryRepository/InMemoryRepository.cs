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
        private List<T> internalStorage = new List<T>();

        public IEnumerable<T> All()
        {
            return internalStorage.AsEnumerable();
        }

        public void Delete(IComparable id)
        {
            var itemToDelete = internalStorage.Find(item => item.Id.CompareTo(id) == 0);
            if (itemToDelete != null)
                internalStorage.Remove(itemToDelete);
        }

        public T FindById(IComparable id)
        {
            return internalStorage.Find(item => item.Id.CompareTo(id) == 0);
        }

        public void Save(T item)
        {
            if (internalStorage.Contains(item)) return;
            internalStorage.Add(item);
        }
    }
}
