using Microsoft.AspNetCore.Identity;

namespace Tests.MockData.Data
{
    /// <inheritdoc />
    /// <summary>
    /// Mocked normalizer for user manager. This is used to normalize values such
    /// as emails when searching for them so lower and upper cases don't matter.
    /// </summary>
    public class MockNormalizer : ILookupNormalizer
    {
        /// <inheritdoc />
        /// <summary>
        /// Returns the key in upper case.
        /// </summary>
        /// <param name="key">Any string</param>
        /// <returns>The provided string in upper case</returns>
        public string Normalize(string key)
        {
            return key.ToUpper();
        }
    }
}
