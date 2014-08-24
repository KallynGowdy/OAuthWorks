using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExampleMvcWebApplication.Models
{
    /// <summary>
    /// Defines a class that represents a <see cref="Value"/> stored by a <see cref="User"/>.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Gets or sets the ID of this value.
        /// </summary>
        /// <returns></returns>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value stored in this object.
        /// </summary>
        /// <returns></returns>
        [Required]
        public string ObjectValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owner of this value. 
        /// </summary>
        /// <returns></returns>
        [Required]
        public virtual User Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets who can see this value.
        /// </summary>
        /// <returns></returns>
        public Visibility Visiblity
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Defines a list of values that define who can view a certian value.
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Defines that anyone can see this value.
        /// </summary>
        Public,

        /// <summary>
        /// Defines that only the owner can see this value.
        /// </summary>
        Private,

        /// <summary>
        /// Defines a that only friends of the owner can see this value.
        /// </summary>
        Friends
    }
}