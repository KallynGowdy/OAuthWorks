using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{

    /// <summary>
    /// Defines a class for objects that contain a token(AuthorizationCode, AccessToken, RefreshToken, ect.) with a generated string representing the token value.
    /// </summary>
    /// <typeparam name="TToken">The type of the tokens that are contained in this object.</typeparam>
    public class CreatedToken<TToken> : ICreatedToken<TToken> where TToken : IToken
    {
        /// <summary>
        /// Creates a new OAuthWorks.CreatedToken object using the given token and token value.
        /// </summary>
        /// <param name="token">The token that was just created.</param>
        /// <param name="tokenValue">The string value that causes <see cref="IToken.MatchesValue(System.String)"/> to evaluate to true.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if either of the given arguments are null.</exception>
        public CreatedToken(TToken token, string tokenValue)
        {
            token.ThrowIfNull("token");
            tokenValue.ThrowIfNull("tokenValue");

            this.Token = token;
            this.TokenValue = tokenValue;
        }

        /// <summary>
        /// Gets the token that was created.
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
