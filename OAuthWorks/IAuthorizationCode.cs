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
    public interface IAuthorizationCode
    {
        /// <summary>
        /// Determines if the given code matches the authorization code that is stored internaly.
        /// </summary>
        /// <param name="code">The authorization code to determine equality to.</param>
        /// <returns>Returns true if the given code matches the internal authorization code, otherwise returns false.</returns>
        bool MatchesCode(string code);

        /// <summary>
        /// Gets whether this authorization code has expired.
        /// </summary>
        bool Expired
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
        /// Defines a utility class for authorization codes.
        /// </summary>
        public static class Util
        {
            /// <summary>
            /// Gets the full authorization code from the given id and code.
            /// </summary>
            /// <param name="id">The Id of the authorization code that is stored in the database.</param>
            /// <param name="authorizationCode">The acutal authorization code.</param>
            /// <returns>Returns the authorization code that should be sent to the client.</returns>
            /// <remarks>
            /// This function is necessary for performance. It allows encoding of the given authorization code properties into a full authorization code that is given
            /// to the client. When the client requests an Access Token with it, it is converted back into the Id an Authorization Code where the server can then retrieve the authorization
            /// code based on the given id and determine validity. For security, this relies on the fact that the client CANNOT guess the Authorization Code of any other authorization code.
            /// It is possible to integrate signing into this model by signing the result with a private key and sending the signed code with the real one. Then, when the client posts the 
            /// code back, you can sign the posted code to see if it matches the signed code.
            /// </remarks>
            public static string GetFullAuthorizationCode(string id, string authorizationCode)
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(id + "-" + authorizationCode));
            }

            /// <summary>
            /// Gets the Id and Authorization Code from the authorization code that was posted from the client.
            /// </summary>
            /// <param name="authorizationCode">The Authorization Code that was sent from the client.</param>
            /// <returns>Returns a <see cref="System.Tuple<string, string>"/> object that contains the Id in the first item and the code in the second slot.
            /// Returns null if the code could not be parsed.</returns>
            /// <remarks>
            /// Use this function to de-encode an authorization code that was retrieved from <see cref="IAuthorizationCode.Util.GetFullAuthorizationCode"/>.
            /// It returns a new <see cref="System.Tuple<string, string>"/> object that represents the values that were given.
            /// </remarks>
            public static Tuple<string, string> GetIdAndAuthorizationCode(string authorizationCode)
            {
                string str = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationCode));
                string[] values = str.Split('-');
                if (values.Length == 2)
                {
                    return new Tuple<string, string>(values[0], values[1]);
                }
                return null;
            }
        }
    }
}
