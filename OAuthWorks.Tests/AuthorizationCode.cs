﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Tests
{
    public class AuthorizationCode : IAuthorizationCode
    {
        public AuthorizationCode(string code, string id, DateTime expirationDateUtc, IEnumerable<IScope> scopes)
        {
            this.code = code;
            this.Id = id;
            this.ExpirationDateUtc = expirationDateUtc.ToUniversalTime();
            this.Scopes = scopes;
        }

        public string Id
        {
            get;
            set;
        }

        private string code;

        public bool MatchesCode(string code)
        {
            return this.code.Equals(code);
        }

        public bool Expired
        {
            get
            {
                return DateTime.UtcNow >= ExpirationDateUtc;
            }
        }

        public IEnumerable<IScope> Scopes
        {
            get;
            set;
        }

        public DateTime ExpirationDateUtc
        {
            get;
            set;
        }
    }
}