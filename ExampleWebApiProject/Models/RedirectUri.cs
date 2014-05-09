using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExampleWebApiProject.Models
{
    /// <summary>
    /// Defines an entity that contains information about a client's aproved redirect uri.
    /// </summary>
    public class RedirectUri
    {
        public RedirectUri()
        {

        }

        public RedirectUri(string uri)
        {
            this.Uri = uri;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public string Uri
        {
            get;
            set;
        }

        public static implicit operator RedirectUri(string uri)
        {
            return new RedirectUri(uri);
        }
    }
}