using OAuthWorks.Factories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.Factories
{
    /// <summary>
    /// Defines a class which provides a basic implementation of <see cref="OAuthWorks.IAuthorizationCodeFactory"/>.
    /// </summary>
    public class AuthorizationCodeFactory : IAuthorizationCodeFactory<HashedAuthorizationCode>
    {
        /// <summary>
        /// The default length of the generated codes in bytes.
        /// </summary>
        /// <value>40</value>
        public const int DefaultCodeLength = 40;

        /// <summary>
        /// The default length of the generated identifiers in bytes.
        /// </summary>
        /// <value>8</value>
        public const int DefaultIdLength = 8;

        /// <summary>
        /// The default lifetime for generated authorization codes.
        /// </summary>
        /// <value>3,600 seconds or 1 hour</value>
        public const int DefaultCodeLifetime = 3600;

        private static readonly Lazy<IValueIdFormatter> lazyFormatter = new Lazy<IValueIdFormatter>(() => new ValueIdFormatter());

        /// <summary>
        /// Gets the default formatter for Ids and tokens.
        /// </summary>
        public static IValueIdFormatter DefaultIdFormatter
        {
            get
            {
                return lazyFormatter.Value;
            }
        }

        /// <summary>
        /// The default pseudorandom value generator.
        /// </summary>
        public static readonly Func<int, string> DefaultValueGenerator = AccessTokenFactory.GenerateToken;

        /// <summary>
        /// Gets the function that, given an integer generates a string that represents that many pseudorandom bytes.
        /// </summary>
        public Func<int, string> ValueGenerator
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length of the codes(in bytes) generated from this factory.
        /// </summary>
        public int CodeLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lifetime of the generated codes in seconds.
        /// </summary>
        public int CodeLifetime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the length (in bytes) of identifiers generated for codes in this factory.
        /// </summary>
        public int IdLength
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the formatter that combines the Id and refreshToken together.
        /// </summary>
        public IValueIdFormatter IdFormatter
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        public AuthorizationCodeFactory()
            : this(DefaultCodeLength, DefaultCodeLifetime, DefaultIdLength)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the authorization codes to generate (in bytes).</param>
        /// <param name="codeLifetime">The number of seconds that the generated codes will be valid for.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        public AuthorizationCodeFactory(int codeLength, int codeLifetime, int idLength) : this(codeLength, codeLifetime, idLength, DefaultIdFormatter, DefaultValueGenerator)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeFactory"/> class.
        /// </summary>
        /// <param name="codeLength">Length of the authorization codes to generate (in bytes).</param>
        /// <param name="codeLifetime">The number of seconds that the generated codes will be valid for.</param>
        /// <param name="idLength">The length (in bytes) of identifiers that are generated from this factory.</param>
        /// <param name="idFormatter">The formatter that combines the generated ids and tokens.</param>
        /// <param name="valueGenerator">A function that, given an integer returns a string that represents that many pseudorandom bytes.</param>
        public AuthorizationCodeFactory(int codeLength, int codeLifetime, int idLength, IValueIdFormatter idFormatter, Func<int, string> valueGenerator)
        {
            Contract.Requires(codeLength >= 20);
            Contract.Requires(codeLifetime > 0);
            Contract.Requires(idLength >= 4);
            Contract.Requires(idFormatter != null);
            Contract.Requires(valueGenerator != null);
            this.CodeLength = codeLength;
            this.CodeLifetime = codeLifetime;
            this.IdLength = idLength;
            this.IdFormatter = idFormatter;
            this.ValueGenerator = valueGenerator;
        }

        /// <summary>
        /// Creates a new <see cref="OAuthWorks.IAuthorizationCode"/> object given the granted scopes.
        /// </summary>
        /// <param name="scopes">The enumerable list of scopes that were granted by the user to the client.</param>
        /// <param name="user">The user that the created authorization code is bound to.</param>
        /// <param name="client">The client that the code is granted for.</param>
        /// <param name="redirectUri">The URI that was provided by the client that specifies the location that the user should be redirected to after completing authorization.</param>
        /// <returns>Returns a new OAuthWorks.CreatedToken(of TAuthorizationCode) object.</returns>
        public ICreatedToken<HashedAuthorizationCode> Create(Uri redirectUri, IUser user, IClient client, IEnumerable<IScope> scopes)
        {
            string token = ValueGenerator(CodeLength);
            string id = ValueGenerator(IdLength);
            string formatted = IdFormatter.FormatValue(id, token);
            DateTime expirationDate = DateTime.UtcNow.AddSeconds(CodeLifetime);
            return new CreatedToken<HashedAuthorizationCode>(new HashedAuthorizationCode(id, formatted, user, client, scopes, redirectUri, expirationDate), formatted);
        }

        public HashedAuthorizationCode Create()
        {
            return null;
        }
    }
}
