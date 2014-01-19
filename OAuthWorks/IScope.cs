using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that contains information on a Scope.
    /// </summary>
    public interface IScope
    {
        // Every scope has an identifier, it is what the client will provide to request access to the resources that the scope controls.

        /// <summary>
        /// Gets the identifier of the scope. This is the value that is refered to in the specification.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Gets or sets the description of the scope. This should be a human-readable summary of what the scope provides.
        /// </summary>
        string Description
        {
            get;
        }
    }
}
