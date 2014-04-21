using OAuthWorks.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class DictionaryRepository<T> : IRepository<T>
    {
        private Dictionary<string, T> dictionary = new Dictionary<string,T>();

        protected IEnumerable<T> Entities
        {
            get
            {
                return dictionary.Values;
            }
        }

        protected Func<T, string> IdSelector
        {
            get;
            set;
        }

        public T GetById(string id)
        {
            T val;
            if (dictionary.TryGetValue(id, out val))
            {
                return val;
            }
            if (val == null)
            {
                return default(T);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        public void Add(T obj)
        {
            dictionary.Add(IdSelector(obj), obj);
        }

        public void Update(T obj)
        {
            if (dictionary.ContainsKey(IdSelector(obj)))
            {
                dictionary[IdSelector(obj)] = obj;
            }
        }

        public void RemoveById(string id)
        {
            dictionary.Remove(id);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dictionary.Values.GetEnumerator();
        }

        public void Dispose()
        {
            
        }
    }
}
