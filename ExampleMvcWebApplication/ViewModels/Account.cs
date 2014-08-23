using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ExampleMvcWebApplication.ViewModels
{
    /// <summary>
    /// Defines a class that represents an input model for a new account.
    /// </summary>
    [DataContract]
    public class Account
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [StringLength(maximumLength: 50)]
        [Required]
        [DataMember(Name = "username")]
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [MinLength(8)]
        [Required]
        [DataMember(Name = "password")]
        public string Password
        {
            get;
            set;
        }
    }
}