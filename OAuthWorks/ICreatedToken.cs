using System;
namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for objects that contain a token(AuthorizationCode, AccessToken, RefreshToken, ect.) with a generated string representing the token value.
    /// </summary>
    /// <typeparam name="TToken">The type of the tokens that are contained in this object.</typeparam>
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
}
