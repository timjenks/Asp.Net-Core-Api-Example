using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Tests.MockData.Data
{
    /// <summary>
    /// Mocking configuration for jwt token keys.
    /// </summary>
    public class MockConfiguration : IConfiguration
    {
        /// <summary>
        /// Access jwt related values (issuer, secret and lifespan).
        /// </summary>
        /// <param name="key">The key for the jwt value</param>
        /// <exception cref="NotImplementedException">Thrown when non-jwt related key is accessed or if set</exception>
        /// <returns>The value of the given key</returns>
        public string this[string key] { get => GetHelper(key); set => throw new NotImplementedException(); }

        /// <summary>
        /// Not meant to be used.
        /// </summary>
        /// <exception cref="NotImplementedException">Alawys thrown</exception>
        /// <returns>Nothing</returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not meant to be used.
        /// </summary>
        /// <exception cref="NotImplementedException">Alawys thrown</exception>
        /// <returns>Nothing</returns>
        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not meant to be used.
        /// </summary>
        /// <exception cref="NotImplementedException">Alawys thrown</exception>
        /// <param name="key"></param>
        /// <returns>Nothing</returns>
        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper for [] access.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown if key not in {Issuer, SecretKey, TokenExpireDays}</exception>
        /// <param name="key">Key for jwt-related value</param>
        /// <returns>Value of key</returns>
        private string GetHelper(string key)
        {
            if (key == "Issuer")
            {
                return "http://put-your-domain.com";
            }
            else if (key == "SecretKey")
            {
                return "58p%__ScZgmBuVRquZfq^8A2?uTUSeuvYm8EFJbcFW%Egz&+N!cRdTcgrwNX4&$3ceXUwPjrH-XC&d9wXEf73%DzLLeQ4RUy*gCvXBsyup=-um=pLz9q9GNNg^8EC?#a";
            }
            else if (key == "TokenExpireDays")
            {
                return "30";
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
