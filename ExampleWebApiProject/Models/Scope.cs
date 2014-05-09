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

using OAuthWorks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ExampleWebApiProject.Models
{
    public class Scope : IScope
    {
        [Key]
        public string Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public virtual ICollection<AuthorizationCode> ReferencedCodes
        {
            get;
            set;
        }

        public virtual ICollection<RefreshToken> ReferencedRefreshTokens
        {
            get;
            set;
        }

        public virtual ICollection<AccessToken> ReferencedAccessTokens
        {
            get;
            set;
        }

        public bool Equals(IScope other)
        {
            return Equals(other as Scope);
        }

        public bool Equals(Scope other)
        {
            return other != null &&
                other.Id == this.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IScope);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() * 23;
        }

        public override string ToString()
        {
            return Id;
        }
    }
}
