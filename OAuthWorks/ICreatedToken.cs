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
using System.Diagnostics.Contracts;
namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for objects that contain a token(AuthorizationCode, AccessToken, RefreshToken, ect.) with a generated string representing the token value.
    /// </summary>
    /// <typeparam name="TToken">The type of the tokens that are contained in this object.</typeparam>
    [ContractClass(typeof(ICreatedTokenContract<>))]
    public interface ICreatedToken<out TToken>
     where TToken : IToken
    {
        /// <summary>
        /// Gets the token that was created.
        /// </summary>
        TToken Token { get; }

        /// <summary>
        /// Gets the value that causes <see cref="IToken.MatchesValue(System.String)"/> to evaluate to true.
        /// </summary>
        string TokenValue { get; }
    }

    [ContractClassFor(typeof(ICreatedToken<>))]
    internal abstract class ICreatedTokenContract<TToken> : ICreatedToken<TToken> where TToken : IToken
    {

        TToken ICreatedToken<TToken>.Token
        {
            get
            {
                Contract.Ensures(Contract.Result<TToken>() != null);
                return default(TToken);
            }
        }

        string ICreatedToken<TToken>.TokenValue
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }
    }
}
