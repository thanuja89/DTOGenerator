using Core;
using Xunit;

namespace DTGen.Tests
{
    public class TSLanguageServiceTests
    {
        private TSLanguageService _sut;

        [Theory]
        [InlineData("int", "number")]
        [InlineData("bool", "boolean")]
        public void GetSimpleType_WhenCSTypeMatched_ReturnsCorrectTSType(string sourceType, string expectedType)
        {
            // Arrange
            SetUp();

            // Act
            var type = _sut.GetSimpleType(sourceType);

            // Assert
            Assert.Equal(expectedType, type);
        }

        [Theory]
        [InlineData("aaa", "aaa")]
        [InlineData("bbb", "bbb")]
        public void GetSimpleType_WhenCSTypeNotMatched_ReturnsSourceType(string sourceType, string expectedType)
        {
            // Arrange
            SetUp();

            // Act
            var type = _sut.GetSimpleType(sourceType);

            // Assert
            Assert.Equal(expectedType, type);
        }

        [Theory]
        [InlineData("string", "string")]
        [InlineData("DateTime", "Date")]
        public void GetPropertyType_WhenSimpleMatchingCSType_ReturnsCorrectTSType(string sourceType, string expectedType)
        {
            // Arrange
            SetUp();

            // Act
            var type = _sut.GetPropertyType(sourceType);

            // Assert
            Assert.Equal(expectedType, type);
        }

        [Theory]
        [InlineData("Task<int>", "Task<number>")]
        [InlineData("Foo<Bar<int>>", "Foo<Bar<number>>")]
        public void GetPropertyType_WhenGenericCSType_ReturnsCorrectGenericTSType(string sourceType, string expectedType)
        {
            // Arrange
            SetUp();

            // Act
            var type = _sut.GetPropertyType(sourceType);

            // Assert
            Assert.Equal(expectedType, type);
        }

        private void SetUp()
        {
            _sut = new TSLanguageService(new GenOptions()
            {
                IsCamelCaseEnabled = true,
                Language = Language.TypeScript
            });
        }

        private void SetUp(GenOptions options)
        {
            _sut = new TSLanguageService(options);
        }
    }
}
