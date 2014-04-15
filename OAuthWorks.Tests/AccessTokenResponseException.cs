using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks.Tests
{
    [Serializable]
    public class AccessTokenResponseException : OAuthWorks.AccessTokenResponseException
    {
        public AccessTokenResponseException(AccessTokenRequestError errorCode, string description, Uri uri, Exception innerException)
            : base(description, innerException)
        {
            this.errorCode = errorCode;
            this.errorDescription = description;
            this.errorUri = uri;
        }

        AccessTokenRequestError errorCode;
        private string errorDescription;
        private Uri errorUri;

        public override AccessTokenRequestError ErrorCode
        {
            get
            {
                return errorCode;
            }
        }

        public override string ErrorDescription
        {
            get
            {
                return errorDescription;
            }
        }

        public override Uri ErrorUri
        {
            get
            {
                return errorUri;
            }
        }
    }
}
