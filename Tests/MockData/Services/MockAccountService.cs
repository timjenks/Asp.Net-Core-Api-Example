using System;
using System.Threading.Tasks;
using TodoApi.Models.ViewModels;
using TodoApi.Services.Interfaces;

namespace Tests.MockData.Services
{
    /// <inheritdoc />
    /// <summary>
    /// A mock of account service.
    /// </summary>
    public class MockAccountService : IAccountService
    {
        /// <summary>
        /// A method to control what Login does. Needs to be set in test.
        /// </summary>
        public Func<LoginViewModel, string> MLogin { get; set; }

        /// <summary>
        /// A method to control what Register does. Needs to be set in test.
        /// </summary>
        public Func<RegisterViewModel, string> MRegister { get; set; }

        /// <inheritdoc />
        public async Task<string> Login(LoginViewModel model)
        {
            await Task.Run(() => { });
            return MLogin(model);
        }

        /// <inheritdoc />
        public async Task<string> Register(RegisterViewModel model)
        {
            await Task.Run(() => { });
            return MRegister(model);
        }
    }
}
