// Copyright 2014 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

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
