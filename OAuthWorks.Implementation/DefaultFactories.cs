using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OAuthWorks.Factories;
using OAuthWorks.Implementation.Factories;

namespace OAuthWorks.Implementation
{
	/// <summary>
	/// Defines a static class that contains constructor functions that create the default factories for a <see cref="OAuthProvider"/>.
	/// </summary>
	public static class DefaultFactories
	{
		/// <summary>
		/// The default <see cref="IAccessTokenFactory{TAccessToken}"/> constructor.
		/// </summary>
		public static readonly Func<IAccessTokenFactory<IAccessToken>> DefaultAccessTokenFactoryConstructor = () => Implementation.Factories.AccessTokenFactory.String.DefaultFactory;

		/// <summary>
		/// The default <see cref="IAuthorizationCodeFactory{TAuthorizationCode}"/> constructor.
		/// </summary>
		public static readonly Func<IAuthorizationCodeFactory<IAuthorizationCode>> DefaultAuthorizationCodeFactoryConstructor = () => Implementation.Factories.AuthorizationCodeFactory.String.DefaultFactory;

		/// <summary>
		/// The default <see cref="IAccessTokenResponseFactory"/> constructor.
		/// </summary>
		public static readonly Func<IAccessTokenResponseFactory> DefaultAccessTokenResponseFactoryConstructor = () => new AccessTokenResponseFactory();

		/// <summary>
		/// The default <see cref="IAuthorizationCodeResponseFactory"/> constructor.
		/// </summary>
		public static readonly Func<IAuthorizationCodeResponseFactory> DefaultAuthorizationCodeResponseFactoryConstructor = () => new AuthorizationCodeResponseFactory();

		/// <summary>
		/// The default <see cref="IRefreshTokenFactory{TRefreshToken}"/> constructor.
		/// </summary>
		public static readonly Func<IRefreshTokenFactory<IRefreshToken>> DefaultRefreshTokenFactoryConstructor = () => Implementation.Factories.RefreshTokenFactory.String.DefaultFactory;


	}
}
