﻿using OAuthWorks.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class DictionaryRepository<K, T> : IRepository<K, T>
    {
        private Dictionary<K, T> dictionary = new Dictionary<K,T>();

        protected IEnumerable<T> Entities
        {
            get
            {
                return dictionary.Values;
            }
        }

        protected Func<T, K> IdSelector
        {
            get;
            set;
        }

        public T GetById(K id)
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

        public void RemoveById(K id)
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
    }
}
