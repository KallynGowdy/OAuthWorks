using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication
{
    /// <summary>
    /// Defines a class that represents a store of http cookies.
    /// </summary>
    public class CookieStore : IDictionary<string, string>
    {
        Dictionary<string, string> cookieDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieStore"/> class.
        /// </summary>
        /// <param name="cookies">The list of cookies that should be put into the store.</param>
        public CookieStore(IEnumerable<string> cookies)
        {
            cookieDictionary = cookies.SelectMany(c => c.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(c =>
                {
                    string trimed = c.Trim();
                    int index = trimed.IndexOf('=');
                    return new KeyValuePair<string, string>(trimed.Substring(0, index), trimed.Substring(index + 1));
                }).ToDictionary(c => c.Key, c => c.Value);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                string str;
                if(cookieDictionary.TryGetValue(key, out str))
                {
                    return str;
                }
                return null;
            }
            set
            {
                cookieDictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count
        {
            get
            {
                return cookieDictionary.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<string> Keys
        {
            get
            {
                return cookieDictionary.Keys;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1" /> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" />.</returns>
        public ICollection<string> Values
        {
            get
            {
                return cookieDictionary.Values;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<string, string> item)
        {
            cookieDictionary.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, string value)
        {
            cookieDictionary.Add(key, value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            cookieDictionary.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, string> item)
        {
            return cookieDictionary.Contains(item);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return cookieDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((IDictionary<string, string>)cookieDictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return cookieDictionary.GetEnumerator();
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<string, string> item)
        {
            return cookieDictionary.Remove(item.Key);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Remove(string key)
        {
            return cookieDictionary.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return cookieDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}