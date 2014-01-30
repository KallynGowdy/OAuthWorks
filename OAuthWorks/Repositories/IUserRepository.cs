using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Repositories
{
    /// <summary>
    /// Defines an interface for a repository of users.
    /// </summary>
    /// <typeparam name="T">The type of the user that will be stored in this repository.</typeparam>
    public interface IUserRepository<T> : IRepository<string, T> where T : IUser
    {


    }
}
