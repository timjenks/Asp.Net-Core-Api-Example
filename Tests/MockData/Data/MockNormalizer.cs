using Microsoft.AspNetCore.Identity;

namespace Tests.MockData.Data
{
    public class MockNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.ToUpper();
        }
    }
}
