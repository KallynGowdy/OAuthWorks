using ExampleMvcWebApplication.Models;
using OAuthWorks;

namespace ExampleMvcWebApplication.Controllers
{
    /// <summary>
    /// Defines an interface for a controller that is apart of the API.
    /// </summary>
    public interface IApiController
    {
        /// <summary>
        /// Gets the <see cref="DatabaseContext"/> used in this controller.
        /// </summary>
        /// <returns>Returns the <see cref="DatabaseContext"/> used by this controller.</returns>
        DatabaseContext Context { get; }

        /// <summary>
        /// Gets the <see cref="IOAuthProvider"/> used by this controller.
        /// </summary>
        /// <returns>Returns the <see cref="IOAuthProvider"/> used by this controller.</returns>
        IOAuthProvider Provider { get; }
    }
}