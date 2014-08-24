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
    /// Defines a class for objects that contain a refreshToken(AuthorizationCode, AccessToken, RefreshToken, ect.) with a generated string representing the refreshToken value.
    /// </summary>
    /// <typeparam name="TToken">The type of the tokens that are contained in this object.</typeparam>
    public class CreatedToken<TToken> : ICreatedToken<TToken> where TToken : IToken
    {
        /// <summary>
        /// Creates a new OAuthWorks.CreatedToken object using the given refreshToken and refreshToken value.
        /// </summary>
        /// <param name="refreshToken">The refreshToken that was just created.</param>
        /// <param name="tokenValue">The string value that causes <see cref="IToken.MatchesValue(System.String)"/> to evaluate to true.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if either of the given arguments are null.</exception>
        public CreatedToken(TToken token, string tokenValue)
        {
            if (token == null) throw new ArgumentNullException("token");
            if (tokenValue == null) throw new ArgumentNullException("tokenValue");

            this.Token = token;
            this.TokenValue = tokenValue;
        }

        /// <summary>
        /// Gets the refreshToken that was created.
        /// </summary>
        public TToken Token
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the value that causes <see cref="IToken.MatchesValue(System.String)"/> to evaluate to true.
        /// </summary>
        public string TokenValue
        {
            get;
            private set;
        }
    }
}
