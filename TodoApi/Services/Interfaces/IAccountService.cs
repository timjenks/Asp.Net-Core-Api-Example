#region Imports

using System.Threading.Tasks;
using TodoApi.Models.ViewModels;

#endregion

namespace TodoApi.Services.Interfaces
{
    /// <summary>
    /// An interface for an account service that defines 
    /// what methods such a service must implement.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Try to login.
        /// </summary>
        /// <param name="model">A model containing login info</param>
        /// <returns>A bearer token</returns>
        Task<string> Login(LoginViewModel model);

        /// <summary>
        /// Try to register.
        /// </summary>
        /// <param name="model">A model containing register info</param>
        /// <returns>A bearer token</returns>
        Task<string> Register(RegisterViewModel model);
    }
}
