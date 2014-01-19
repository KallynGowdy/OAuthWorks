using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a repository.
    /// </summary>
    /// <typeparam name="T">The type of the entities that are stored in this repository.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>Returns the requested entity.</returns>
        T Get(object id);

        /// <summary>
        /// Adds the given object 
        /// </summary>
        /// <param name="obj"></param>
        void Add(T obj);

    }
}
