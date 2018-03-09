#region Imports

using System;
using TodoApi.Utils.TimeUtils;
using Xunit;

#endregion

namespace Tests.UtilsTests.TimeUtils
{
    /// <summary>
    /// Test for the util class QueryDateBuilder.
    /// </summary>
    public class QueryDateBuilderTest
    {
        [Fact]
        public void CreateDate_NullParameter_Null()
        {
            Assert.Null(QueryDateBuilder.CreateDate(null, "1", "1"));
            Assert.Null(QueryDateBuilder.CreateDate("1", null, "1"));
            Assert.Null(QueryDateBuilder.CreateDate("1", "1", null));
        }

        [Fact]
        public void CreateDate_NonIntegerParameter_Null()
        {
            Assert.Null(QueryDateBuilder.CreateDate("A", "1", "1"));
            Assert.Null(QueryDateBuilder.CreateDate("1", "A", "1"));
            Assert.Null(QueryDateBuilder.CreateDate("1", "1", "A"));
        }

        [Fact]
        public void CreateDate_InvalidParameter_Null()
        {
            // Arrange
            const string year = "2015";
            const string month = "2";
            const string day = "29";

            // Act
            var date = QueryDateBuilder.CreateDate(year, month, day);

            // Assert
            Assert.Null(date);
        }

        [Fact]
        public void CreateDate_ValidParameter_Matches()
        {
            // Arrange
            const string year = "2016";
            const string month = "2";
            const string day = "29";

            // Act
            var date = QueryDateBuilder.CreateDate(year, month, day);

            // Assert
            Assert.Equal(new DateTime(2016, 2, 29), date);
        }
    }
}