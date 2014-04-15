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



using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for authorization code objects.
    /// </summary>
    /// <remarks>
    /// Authorization codes should be stored like any other password. Even though they expire, the information that they contain could still be used
    /// to access the user's account. OAuthWorks formats Authorization Codes by including the Id in the response. This way we can access the code with
    /// the information in the request easily without requiring extra information. This does not open any security holes if the tokens are hashed and
    /// have a small expiration date with a possible one-time switch.
    /// </remarks>
    public interface IAuthorizationCode : IToken
    {
        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        bool Expired
        {
            get;
        }

        /// <summary>
        /// Gets the redirect Uri that was used by the client when retrieving this token.
        /// </summary>
        Uri RedirectUri
        {
            get;
        }

        /// <summary>
        /// Gets the scopes that this code grants access to.
        /// </summary>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Gets the expiration date of this authorization code in Universal Coordinated Time.
        /// </summary>
        DateTime ExpirationDateUtc
        {
            get;
        }

        /// <summary>
        /// Gets the user that this authorization code belongs to.
        /// </summary>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the client that this authorization code was granted to.
        /// </summary>
        IClient Client
        {
            get;
        }
    }

    internal abstract class IAuthorizationCodeContract : IAuthorizationCode
    {
        bool IAuthorizationCode.Expired
        {
            get
            {
                Contract.Ensures(!Contract.Result<bool>() && DateTime.UtcNow < ((IAuthorizationCode)this).ExpirationDateUtc);
                return default(bool);
            }
        }

        Uri IAuthorizationCode.RedirectUri
        {
            get { return default(Uri); }
        }

        IEnumerable<IScope> IAuthorizationCode.Scopes
        {
            get { return default(IEnumerable<IScope>); }
        }

        DateTime IAuthorizationCode.ExpirationDateUtc
        {
            get { return default(DateTime); }
        }

        bool IToken.MatchesValue(string token)
        {
            return default(bool);
        }

        IUser IAuthorizationCode.User
        {
            get
            {
                Contract.Ensures(Contract.Result<IUser>() != null);
                return default(IUser);
            }
        }

        IClient IAuthorizationCode.Client
        {
            get
            {
                Contract.Ensures(Contract.Result<IClient>() != null);
                return default(IClient);
            }
        }
    }
}
