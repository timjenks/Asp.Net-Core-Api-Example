using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Tests.MockData.Data
{
    public class MockConfiguration : IConfiguration
    {
        public string this[string key] { get => GetHelper(key); set => throw new NotImplementedException(); }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }

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
