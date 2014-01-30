using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for a User of an OAuth 2.0 provider.
    /// </summary>
    public interface IUser
    {
        // The User needs to be able to retrieve information such as,
        // 
        // - Id
        // - Granted Permissions

        /// <summary>
        /// Gets the Id of the user.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Determines if the given scope has been granted to the given client by this user.
        /// </summary>
        /// <param name="client">The client that is used to determine whether it has been granted the scope by this user.</param>
        /// <param name="scope">The scope that may or may not have been granted to the given client.</param>
        /// <returns>Returns true if the given client has been granted the given scope, otherwise false.</returns>
        bool HasGrantedScope(IClient client, IScope scope);
    }
}
