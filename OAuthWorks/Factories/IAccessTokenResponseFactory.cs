using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for a <see cref="OAuthWorks.IFactory"/> object that produces <see cref="OAuthWorks.IAccessTokenResponse"/> objects.
    /// </summary>
    public interface IAccessTokenResponseFactory<in TAccessTokenResponse> : IFactory<TAccessTokenResponse> where TAccessTokenResponse : IAccessTokenResponse
    {
        TAccessTokenResponse Get(string accessToken, 

    }
}
