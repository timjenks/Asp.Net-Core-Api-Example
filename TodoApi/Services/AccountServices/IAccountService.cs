using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models.ViewModels;

namespace TodoApi.Services.AccountServices
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="model">TODO</param>
        /// <returns>TODO</returns>
        Task<string> Login(LoginViewModel model);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="model">TODO</param>
        /// <returns>TODO</returns>
        Task<string> Register(RegisterViewModel model);
    }
}
