#OAuthWorks
===============================================================================================================

A simple and extensible OAuth Provider for C#

Nowadays most every web application could benefit from OAuth. Even coporate businesses! OAuth allows applications to interact in a secure, authorized, role-based way without comprimizing the user's credentials or account. Plus, it allows the centralization of authentication, something which could help users mainain better control of their information. This could benefit applications in many unforseen ways. 

*OAuthWorks* provides the ablility to authorize applications to access certian parts of a user's account. It acheieves this through implementing the [OAuth 2.0][oauth] protocol. Because it is meant to be extensible, *OAuthWorks* does not handle authentication of users, only clients. This allows the application to perform any sort of authentication required. Whether it's two-factor authentication or if it in turn requrires users to log in with another OAuth provider, *OAuthWorks* allows it all to happen. This requires some creative thinking as the library needs to interact with user information without knowing how authentication is implemented. This is why *OAuthWorks* is designed using interfaces. It allows for your implementation to seamlessly integrate with *OAuthWorks*. As you might expect, there are some interfaces you should know about, so here it goes:

###Data Structure
Because *OAuthWorks* is designed to be platform-agnostic, it's data structure needs to be designed for such. Therefore, any dependency contained in the platform is described using interfaces. This allows for *OAuthWorks* to function in almost any situation, including across frameworks such as MVC, Web API or even ServiceStack. As such, there are several interfaces that need to be implemented in order to interface with your data structure.

###Program Flow
*OAuthWorks* tries to follow the [OAuth 2.0 spec][oauth] as closely as possible to avoid any sort of confusion. Therefore, there are only a couple things that you need to familiarize yourself with before you get started. The first thing is to become familiar with all of the [OAuth 2.0][oauth] [authorization grant flows](http://tools.ietf.org/html/rfc6749#section-1.3). Second, learn the equivalent flow used by *OAuthWorks*.

The [OAuth 2.0 spec][oauth] contains 3 different ways for a client to obtain an access token for a user's account.
- [Authorization Code](http://tools.ietf.org/html/rfc6749#section-1.3.1)
- [Implicit](http://tools.ietf.org/html/rfc6749#section-1.3.2)
- [Resource Owner Password Credentials](http://tools.ietf.org/html/rfc6749#section-1.3.3)

Each of these ways ways result in one specific thing: **An [Access Token](http://tools.ietf.org/html/rfc6749#section-1.4) is granted to the client.**

How that is achieved is specific to the flow. *OAuthWorks* handles all of these in one class, *OAuthProvider*. Tokens are issued through the `RequestAccessToken` methods which take a request, validate the client and issue a coresponding access token. *OAuthProvider* instances should be created on a per request basis as should repositories. This allows for the simple management of transactions as a [unit-of-work][unit-of-work]. Contrary to many designs, repositories in *OAuthWorks* live inside a [unit-of-work][unit-of-work], removing any need for `SaveChanges` type of methods. As such, every repository must be disposable to ensure proper handling of any sort of database transactions. Therefore, handling an access token request in *OAuthWorks* using Web API v2 and Entity Framework would look like this:

```csharp
    [Route("api/oauth2/accessToken")] // The route that this method will be accessed at
    [HttpPost]
    public HttpResponseMessage RequestAccessToken(AuthorizationCodeGrantAccessTokenRequest request)
    {
        using(DatabaseContext context = new DatabaseContext) // Your EF DataContext (Unit of work/transaction)
        using(OAuthProvider provider = new OAuthProvider
                {
                    // Custom-built repositories for EF
                    AccessTokenRepostory = new AccessTokenRepository(context), 
                    AuthorizationCodeRepository = new AuthorizationCodeRepository(context),
                    ClientRepository = new ClientRepository(context),
                    ScopeRepository = new ScopeRepository(context)
                })
        {
            IAccessTokenResponse response = provider.RequestAccessToken(request); // Send request to provider for handling
            context.SaveChanges(); // Save any data that has been added/manipulated
            return Request.CreateResponse(response.StatusCode(), response); // Return a message with the proper HttpStatusCode
        } // All data is saved to the database as a unit and 
          // unneeded information is disposed.
    }
```
    
That method handles all HTTP POST request to the url: `http://yourwebsite.com/api/oauth2/accessToken` and returns valid responses that adhere to the [OAuth 2.0 spec][oauth].

Now, let's examine what's going on here:

- First, we initalize the [unit-of-work][unit-of-work]. This provides the transaction management for all of our data manipulation. If anything fails before `SaveChanges()`, the database will remain in the state that it was in before the request happened. This atomicity is very valuable to ensure that the database is always in a consistant and valid state.

- Second, we initialize our `OAuthProvider`. We initialize it with all of the required repositories for the particular request, in this case those are
    - The Access Token Repository, used for storing the newly issued access token (not refresh token).
    - Authorization Code Repository, used for retrieving and therefore validating the issued authoriztion code.
    - Client Repository, used for retrieving and therefore validating the client.
    - Scope Repository, used for validating and providing access to the granted scopes to the client though the issued token.

- Third, process the request and get the response. We send any object that implements `IAuthorizationCodeGrantAccessTokenRequest` to the provider and it will build a valid response to that request. This is done by using a combination of the repositories and factories. The repositories are used to retrieve and store information. The factories are used to generate new information, like new access tokens or authorization codes. *OAuthWorks* provides sensible defaults for factories for you, while still letting you determine how the information should be stored. You can provide your own versions by implementing the `IYYYFactory` interfaces. For example, if you wanted to provide your own Authorization Code generation mechanizm, you would implement `IAuthorizationCodeFactory` and provide an instance to the `OAuthProvider`.

```csharp
        public class AuthorizationCodeFactory : OAuthWorks.IAuthorizationCodeFactory<MyAuthorizationCodeImplementation>
        {
            public ICreatedToken<MyAuthorizationCodeImplementation> Create(Uri redirectUri, 
                                                                           IUser user, 
                                                                           IClient client,
                                                                           IEnumerable<IScope> scopes)
            {
                //Generate the code according to your own rules.
                string code = new Random().Next(0, int.MaxValue).ToString(); // Non-secure, but proves the point.

                // Create the token according to your own buisness rules, all the users, clients and scopes
                // are only provided from your own repositories, so casting is fine.
                return new CreatedToken(code, new MyAuthorizationCodeImplementation(code: code, 
                                                                                    user: user,
                                                                                    client: client,
                                                                                    redirectUri: redirectUri,
                                                                                    scopes: scopes));
            }
            
            public ICreatedToken<MyAuthorizationCodeImplementation> Create()
            {
                return null; // Or whatever is default for your implementation. Not used as of yet.
            }
        }
```

- Fourth and finally, we prepare the response and return it to the client.


[oauth]: (http://tools.ietf.org/html/rfc6749)
[unit-of-work]:(http://msdn.microsoft.com/en-us/library/ff649690.aspx?ppud=4)
