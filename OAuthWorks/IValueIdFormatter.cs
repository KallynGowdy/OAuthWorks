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
    /// Defines an interface for an object that formats a given id into a value such that the item is then retrievable by it's value.
    /// </summary>
    /// <remarks>
    /// In the OAuth 2.0 protocol, the client is only required to provide the actual value of the access token or authorization code.
    /// In order to store the tokens/codes securely, they need to be hashed. However, that would make retrieving the tokens/codes
    /// impossible to do efficiently. Therefore the simple solution is to embed the Id in the token/code that we give the client.
    /// The fact that the token/code values are hashed means that no security holes are intruduced by implementing this improvement.
    /// </remarks>
    public interface IValueIdFormatter
    {
        /// <summary>
        /// Formats the given Id and token into one value that is returned.
        /// </summary>
        /// <param name="id">The Id that should be integrated into the given token.</param>
        /// <param name="token">The token that the Id should be integrated into.</param>
        /// <returns>Returns a new string that, contains both the given token and Id in a way that they're both easily retrievable.</returns>
        string FormatValue(string id, string token);

        /// <summary>
        /// Gets the Id value that is stored in the given formatted value.
        /// </summary>
        /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)"/>.</param>
        /// <returns>Returns the Id value that was stored in the given formatted value.</returns>
        string GetId(string formattedToken);

        /// <summary>
        /// Gets the token value that is stored in the given formatted value.
        /// </summary>
        /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)"/>.</param>
        /// <returns>Returns the token value that was stored in the given formatted value.</returns>
        string GetToken(string formattedToken);
    }

    [ContractClassFor(typeof(IValueIdFormatter))]
    internal abstract class IValueIdFormatterContract : IValueIdFormatter
    {
        string IValueIdFormatter.FormatValue(string id, string token)
        {
            Contract.Requires(id != null);
            Contract.Requires(token != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return default(string);
        }

        string IValueIdFormatter.GetId(string formattedToken)
        {
            Contract.Requires(formattedToken != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return default(string);
        }

        string IValueIdFormatter.GetToken(string formattedToken)
        {
            Contract.Requires(formattedToken != null);
            Contract.Ensures(Contract.Result<string>() != null);
            return default(string);
        }
    }
}
